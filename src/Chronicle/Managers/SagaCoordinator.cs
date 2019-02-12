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

        public SagaCoordinator(
            ISagaLog log,
            ISagaStateRepository repository,
            ISagaSeeker seeker)
        {
            _log = log;
            _repository = repository;
            _seeker = seeker;
        }

        public async Task ProcessAsync<TMessage>(TMessage message, ISagaContext context = null) where TMessage : class
            => await ProcessAsync(null, message, context);

        public async Task ProcessAsync<TMessage>(Guid id, TMessage message, ISagaContext context = null) where TMessage : class
            => await ProcessAsync((Guid?) id, message, context);

        private async Task ProcessAsync<TMessage>(Guid? id, TMessage message, ISagaContext context = null) where TMessage : class
        {
            var actions = _seeker.Seek<TMessage>().ToList();
            var sagaTasks = new List<Task>();

            foreach (var action in actions)
            {
                sagaTasks.Add(ProcessAsync(id, message, action, context));
            }

            await Task.WhenAll(sagaTasks);
        }

        private async Task ProcessAsync<TMessage>(
            Guid? nid, TMessage message, ISagaAction<TMessage> action, ISagaContext context = null) where TMessage : class
        {
            context = context ?? SagaContext.Empty;
            var saga = (ISaga)action;
            var sagaType = saga.GetType();
            var id = nid ?? saga.ResolveId(message, context);
            var dataType = saga.GetSagaDataType();
            var state = await _repository.ReadAsync(id, sagaType).ConfigureAwait(false);

            if (state is null)
            {
                if (!(action is ISagaStartAction<TMessage>))
                {
                    return;
                }

                state = CreateSagaState(id, sagaType, dataType);
            }
            else if(state.State == SagaStates.Rejected)
            {
                return;
            }

            InitializeSaga(saga, id, state);

            var isError = false;

            try
            {
                await action.HandleAsync(message, context);
            }
            catch
            {
                isError = true;
            }

            var updatedSagaData = saga
                .GetType()
                .GetProperty(nameof(ISaga<object>.Data))
               ?.GetValue(saga);

            state.Update(saga.State, updatedSagaData);
            var logData = SagaLogData.Create(id, sagaType, message);

            var persistenceTasks = new Task[2]
            {
                _repository.WriteAsync(state),
                _log.WriteAsync(logData)
            };

            await Task.WhenAll(persistenceTasks).ConfigureAwait(false);

            if (saga.State is SagaStates.Rejected || isError)
            {
                await CompensateAsync(saga, sagaType, context);
            }
        }

        private static ISagaState CreateSagaState(Guid id, Type sagaType, Type dataType)
        {
            var sagaData = dataType != null ? Activator.CreateInstance(dataType) : null;
            return SagaState.Create(id, sagaType, SagaStates.Pending, sagaData);
        }

        private void InitializeSaga(ISaga saga, Guid id, ISagaState state)
        {
            if (state.Data is null)
            {
                saga.Initialize(id, state.State);
            }
            else
            {
                saga.InvokeGeneric(nameof(ISaga<object>.Initialize), id, state, state.Data);
            }
        }

        private async Task CompensateAsync(ISaga saga, Type sagaType, ISagaContext context)
        {
            var sagaLogs = await _log.ReadAsync(saga.Id, sagaType);
            sagaLogs
                .OrderByDescending(l => l.CreatedAt)
                .Select(l => l.Message)
                .ToList()
                .ForEach(async message =>
                {
                    ((Task)saga.InvokeGeneric(nameof(ISagaAction<object>.CompensateAsync), message, context))
                        .ConfigureAwait(false);
                });
        }
    }
}
