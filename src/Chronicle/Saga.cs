using System;
using System.Threading.Tasks;
using Chronicle.Persistance;
using Chronicle.Sagas;

namespace Chronicle
{
    public abstract class Saga<TData> : ISaga<TData>, ISagaPersister<TData> where TData : class, ISagaData, new()
    {
        public TData Data { get; protected set; }
        
        private ISagaDataRepository<TData> _repository;

        public SagaStates GetState()
            => Data.State;

        void ISagaPersister<TData>.SetPersister(ISagaDataRepository<TData> repository)
            => _repository = repository;

        async Task ISagaPersister.ReadAsync(Guid id)
        {
            var data = await _repository.ReadAsync(id);

            if (data is null)
            {
                data = new TData
                {
                    Id = id,
                    State = SagaStates.Pending
                };
            }

            Data = data;
        }

        async Task ISagaPersister.WriteAsync()
            => await _repository.WriteAsync(Data);

        async Task ISagaPersister.CompleteAsync()
        {
            Data.State = SagaStates.Completed;
            await ((ISagaPersister)this).WriteAsync();
        }

        async Task ISagaPersister.RejectAsync()
        {
            Data.State = SagaStates.Rejected;
            await ((ISagaPersister)this).WriteAsync();
        }
    }
}
