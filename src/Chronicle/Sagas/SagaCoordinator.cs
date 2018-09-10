using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chronicle.Persistence;

namespace Chronicle.Sagas
{
    internal class SagaCoordinator : ISagaCoordinator
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

        public async Task ProcessAsync<TMessage>(Guid id, TMessage message) where TMessage : class
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

                sagaTasks.Add(ProcessAsync(id, sagaType, sagaDataType, message, action));
            }

            await Task.WhenAll(sagaTasks);
        }

        private async Task ProcessAsync<TMessage>(Guid id, Type sagaType, Type sagaDataType, TMessage message, ISagaAction<TMessage> action) where TMessage : class
        {
            var sagaData = await _repository.ReadAsync(id, sagaType).ConfigureAwait(false);

            var saga = (ISaga)action;

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

            await action.HandleAsync(message);

            var newSagaData = SagaData.Create(id, sagaType, saga.State, saga.Data);
            var sagaLogData = SagaLogData.Create(id, sagaType, message);

            var persistanceTasks = new Task[2]
            {
                _repository.WriteAsync(newSagaData),
                _sagaLog.SaveAsync(sagaLogData)
            };

            await Task.WhenAll(persistanceTasks);

            if (saga.State is SagaStates.Rejected)
            {
                await CompensateAsync(saga, sagaType);
            }
        }


        private async Task CompensateAsync(ISaga saga, Type sagaType)
        {
            var sagaLogDatas = await _sagaLog.GetAsync(saga.Id, sagaType);
            sagaLogDatas
                .OrderByDescending(sld => sld.CreatedAt)
                .Select(sld => sld.Message)
                .ToList()
                .ForEach(async m =>
                {
                    var messageType = m.GetType();

                    await (Task) sagaType
                    .GetMethod(nameof(ISagaAction<object>.CompensateAsync), new[] { messageType })
                    .Invoke(saga, new[] { m });
                });
        }
    }
}
