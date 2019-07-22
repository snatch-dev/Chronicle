using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chronicle.Persistence;
using Chronicle.Utils;

namespace Chronicle.Managers
{
    internal sealed class SagaCoordinator : ISagaCoordinator
    {
        private readonly ISagaLog _log;
        private readonly ISagaStateRepository _repository;
        private readonly ISagaSeeker _seeker;
        private readonly KeyedSemaphore _locker = new KeyedSemaphore();

        public SagaCoordinator(ISagaLog log, ISagaStateRepository repository, ISagaSeeker seeker)
        {
            _log = log;
            _repository = repository;
            _seeker = seeker;
        }

        public Task ProcessAsync<TMessage>(TMessage message, ISagaContext context = null) where TMessage : class =>
            ProcessAsync(message: message, onCompleted: null, onRejected: null, context: context);

        public async Task ProcessAsync<TMessage>(TMessage message, Func<TMessage, ISagaContext, Task> onCompleted = null,
            Func<TMessage, ISagaContext, Task> onRejected = null, ISagaContext context = null) where TMessage : class
        {
            var actions = _seeker.Seek<TMessage>().ToList();
            var sagaTasks = new List<Task>();

            Task EmptyHook(TMessage m, ISagaContext ctx) => Task.CompletedTask;
            onCompleted = onCompleted ?? EmptyHook;
            onRejected = onRejected ?? EmptyHook;

            foreach (var action in actions)
            {
                sagaTasks.Add(ProcessAsync(message, action, onCompleted, onRejected, context));
            }

            await Task.WhenAll(sagaTasks);
        }

        private async Task ProcessAsync<TMessage>(TMessage message, ISagaAction<TMessage> action,
            Func<TMessage, ISagaContext, Task> onCompleted, Func<TMessage, ISagaContext, Task> onRejected,
            ISagaContext context = null) where TMessage : class
        {
            context = context ?? SagaContext.Empty;
            var saga = (ISaga)action;
            var sagaType = saga.GetType();
            var id = saga.ResolveId(message, context);
            var dataType = saga.GetSagaDataType();

            using (await _locker.LockAsync(id))
            {
                var state = await _repository.ReadAsync(id, sagaType).ConfigureAwait(false);

                if (state is null)
                {
                    if (!(action is ISagaStartAction<TMessage>))
                    {
                        return;
                    }

                    state = CreateSagaState(id, sagaType, dataType);
                }
                else if (state.State == SagaStates.Rejected)
                {
                    return;
                }

                InitializeSaga(saga, id, state);

                try
                {
                    await action.HandleAsync(message, context);
                }
                catch (Exception e)
                {
                    context.SagaContextError = new SagaContextError(e);
                    saga.Reject();
                }

                await UpdateSagaAsync(message, saga, state);

                if (saga.State is SagaStates.Rejected)
                {
                    await onRejected(message, context);
                    await CompensateAsync(saga, sagaType, context);
                }
                else if (saga.State is SagaStates.Completed)
                {
                    await onCompleted(message, context);
                }
            }
        }

        private static ISagaState CreateSagaState(SagaId id, Type sagaType, Type dataType)
        {
            var sagaData = dataType != null ? Activator.CreateInstance(dataType) : null;
            return SagaState.Create(id, sagaType, SagaStates.Pending, sagaData);
        }

        private void InitializeSaga(ISaga saga, SagaId id, ISagaState state)
        {
            if (state.Data is null)
            {
                saga.Initialize(id, state.State);
            }
            else
            {
                saga.InvokeGeneric(nameof(ISaga<object>.Initialize), id, state.State, state.Data);
            }
        }

        private async Task UpdateSagaAsync<TMessage>(TMessage message, ISaga saga, ISagaState state)
            where TMessage : class
        {
            var sagaType = saga.GetType();

            var updatedSagaData = sagaType.GetProperty(nameof(ISaga<object>.Data))?.GetValue(saga);

            state.Update(saga.State, updatedSagaData);
            var logData = SagaLogData.Create(saga.Id, sagaType, message);

            var persistenceTasks = new Task[2] { _repository.WriteAsync(state), _log.WriteAsync(logData) };

            await Task.WhenAll(persistenceTasks).ConfigureAwait(false);
        }

        private async Task CompensateAsync(ISaga saga, Type sagaType, ISagaContext context)
        {
            var sagaLogs = await _log.ReadAsync(saga.Id, sagaType);
            sagaLogs.OrderByDescending(l => l.CreatedAt)
                .Select(l => l.Message)
                .ToList()
                .ForEach(async message =>
                {
                    await ((Task)saga.InvokeGeneric(nameof(ISagaAction<object>.CompensateAsync), message, context))
                        .ConfigureAwait(false);
                });
        }
    }
}
