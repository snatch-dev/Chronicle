using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chronicle.Persistence
{
    internal class InMemorySagaDataRepository<TData> : ISagaDataRepository<TData> where TData : class
    {
        private readonly List<ISagaData<TData>> _repository;

        public InMemorySagaDataRepository()
            => _repository = new List<ISagaData<TData>>();

        public async Task<ISagaData<TData>> ReadAsync(Guid sagaId)
            => await Task.FromResult(_repository.FirstOrDefault(sd => sd.SagaId == sagaId));

        public async Task WriteAsync(ISagaData<TData> sagaData)
        {
            var sagaDataToUpdate = await ReadAsync(sagaData.SagaId);

            _repository.Remove(sagaDataToUpdate);
            _repository.Add(sagaData);

            await Task.CompletedTask;
        }
    }
}
