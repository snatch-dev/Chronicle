using System;
using System.Linq;
using System.Threading.Tasks;
using Chronicle.Persistence;

namespace Chronicle.Sagas
{
    internal class SagaCoordinator<TSaga, TData> : ISagaCoordinator<TSaga, TData>  where TSaga : ISaga<TData> where TData : class, new()
    {
        private readonly TSaga _saga;
        private readonly ISagaDataRepository<TData> _repository;
        private readonly ISagaLog _sagaLog;

        public SagaCoordinator(
            TSaga saga,
            ISagaDataRepository<TData> repository,
            ISagaLog sagaLog)
        {
            _saga = saga;
            _repository = repository;
            _sagaLog = sagaLog;
        }

        public async Task DispatchAsync<TMessage>(Guid id, TMessage message) where TMessage : class
        {
            var sagaData = await _repository.ReadAsync(id);

            if(sagaData is null)
            {
                sagaData = SagaData<TData>.Create(id, SagaStates.Pending, new TData());
            }

            _saga.Initialize(sagaData.SagaId, sagaData.State, sagaData.Data);

            if(_saga is ISagaAsyncAction<TMessage> action)
            {
                await action.HandleAsync(message);

                var newSagaData = SagaData<TData>.Create(id, _saga.State, _saga.Data);
                var sagaLogData = SagaLogData.Create(_saga.Id, message);

                var persistanceTasks = new Task[2]
                {
                    _repository.WriteAsync(newSagaData),
                    _sagaLog.SaveAsync(sagaLogData)
                };

                await Task.WhenAll(persistanceTasks);

                if (_saga.State is SagaStates.Rejected)
                {
                    await CompensateAsync();
                }
            }
            else
            {
                throw new ChronicleException("Saga does not handle given type of message.");
            }
        }

        private async Task CompensateAsync()
        {
            var sagaLogDatas = await _sagaLog.GetAsync(_saga.Id);
            sagaLogDatas
                .OrderByDescending(sld => sld.CreatedAt)
                .Select(sld => sld.Message)
                .ToList()
                .ForEach(async m =>
                {
                    var messageType = m.GetType();
                    var sagaType = _saga.GetType();

                    await (Task) sagaType
                    .GetMethod(nameof(ISagaAsyncAction<object>.CompensateAsync), new[] { messageType })
                    .Invoke(_saga, new[] { m });
                });
        }
    }
}
