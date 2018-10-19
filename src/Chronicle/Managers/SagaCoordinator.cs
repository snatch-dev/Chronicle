using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chronicle.Persistence;

namespace Chronicle.Managers
{
    internal sealed class SagaCoordinator : ISagaCoordinator
    {
        private readonly ISagaLog _sagaLog;
        private readonly ISagaDataRepository _repository;
        private readonly ISagaSeeker _sagaSeeker;

        public SagaCoordinator(
            ISagaLog sagaLog,
            ISagaDataRepository repository,
            ISagaSeeker sagaSeeker)
        {
            _sagaLog = sagaLog;
            _repository = repository;
            _sagaSeeker = sagaSeeker;
        }

        public async Task ProcessAsync<TMessage>(TMessage message, ISagaContext context = null) where TMessage : class
            => await ProcessAsync(null, message, context);

        public async Task ProcessAsync<TMessage>(Guid id, TMessage message, ISagaContext context = null) where TMessage : class
            => await ProcessAsync((Guid?) id, message, context);

        private async Task ProcessAsync<TMessage>(Guid? id, TMessage message, ISagaContext context = null) where TMessage : class
        {
            var actions = _sagaSeeker.Seek<TMessage>().ToList();
            var sagaTasks = new List<Task>();

            foreach (var action in actions)
            {
                var sagaType = action.GetType();
                var sagaDataType = action
                    .GetType()
                    .GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISaga<>))
                    .GetGenericArguments()
                    .First();

                sagaTasks.Add(ProcessAsync(id, sagaType, sagaDataType, message, action, context));
            }

            await Task.WhenAll(sagaTasks);
        }

        private async Task ProcessAsync<TMessage>(Guid? nid, Type sagaType, Type sagaDataType, TMessage message, ISagaAction<TMessage> action, ISagaContext context = null) where TMessage : class
        {
            context = context ?? SagaContext.Empty;

            var saga = (ISaga)action;            
            var id = nid ?? saga.ResolveId(message, context);
            var sagaData = await _repository.ReadAsync(id, sagaType).ConfigureAwait(false);            

            if (sagaData is null)
            {
                if (!(action is ISagaStartAction<TMessage>))
                {
                    return;
                }
                sagaData = SagaData.Create(id, sagaType, SagaStates.Pending, Activator.CreateInstance(sagaDataType));
            }
            else if(sagaData.State == SagaStates.Rejected)
            {
                return;
            }

            saga.Initialize(sagaData.SagaId, sagaData.State, sagaData.Data);

            var isError = false;

            try
            {
                await action.HandleAsync(message, context);
            }
            catch
            {
                isError = true;
            }

            var newSagaData = SagaData.Create(id, sagaType, saga.State, saga.Data);
            var sagaLogData = SagaLogData.Create(id, sagaType, message);

            var persistanceTasks = new Task[2]
            {
                _repository.WriteAsync(newSagaData),
                _sagaLog.SaveAsync(sagaLogData)
            };

            await Task.WhenAll(persistanceTasks).ConfigureAwait(false);

            if (saga.State is SagaStates.Rejected || isError)
            {
                await CompensateAsync(saga, sagaType, context);
            }
        }

        private async Task CompensateAsync(ISaga saga, Type sagaType, ISagaContext context)
        {
            var sagaLogDatas = await _sagaLog.GetAsync(saga.Id, sagaType);
            sagaLogDatas
                .OrderByDescending(sld => sld.CreatedAt)
                .Select(sld => sld.Message)
                .ToList()
                .ForEach(async message =>
                {
                    var messageType = message.GetType();
                    var contextType = context.GetType();

                    await ((Task) sagaType
                        .GetMethod(nameof(ISagaAction<object>.CompensateAsync), new[] { messageType, contextType })
                        .Invoke(saga, new[] { message, context }))
                    .ConfigureAwait(false);
                });
        }
    }
}
