using System;
using System.Threading.Tasks;

namespace Saga
{
    public interface ISaga
    {
        SagaStates GetState();
        Task ReadAsync(Guid id);
        Task WriteAsync();
        Task CompleteAsync();
        Task RejectAsync();
    }

    public interface ISaga<TData> : ISaga  where TData : ISagaData
    {
        TData Data { get; }
    }

    public abstract class BaseSaga<TData> : ISaga<TData> where TData : class, ISagaData, new()
    {
        public TData Data { get; protected set; }
        
        private readonly ISagaDataRepository<TData> _repository;

        protected BaseSaga(ISagaDataRepository<TData> repository)
            => (_repository) = (repository);

        public SagaStates GetState()
            => Data.State;

        public virtual async Task ReadAsync(Guid id)
        {
            var data = await _repository.ReadAsync(id);

            if(data is null)
            {
                data = new TData();
                data.Id = id;
                data.State = SagaStates.Pending;
            }

            Data = data;
        }

        public virtual async Task WriteAsync()
            => await _repository.WriteAsync(Data);

        public async Task CompleteAsync()
        {
            Data.State = SagaStates.Finished;
            await WriteAsync();
        }

        public async Task RejectAsync()
        {
            Data.State = SagaStates.Canceled;
            await WriteAsync();
        }
    }
}
