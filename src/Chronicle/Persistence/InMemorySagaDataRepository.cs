using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chronicle.Persistence
{
    internal class InMemorySagaDataRepository : ISagaDataRepository
    {
        private readonly List<ISagaData> _repository;

        public InMemorySagaDataRepository()
            => _repository = new List<ISagaData>();

        public async Task<ISagaData> ReadAsync(Guid sagaId, Type sagaType)
            => await Task.FromResult(_repository.FirstOrDefault(s => s.SagaId == sagaId && s.SagaType == sagaType));

        public async Task WriteAsync(ISagaData sagaData)
        {
            var sagaDataToUpdate = await ReadAsync(sagaData.SagaId, sagaData.SagaType);

            _repository.Remove(sagaDataToUpdate);
            _repository.Add(sagaData);

            await Task.CompletedTask;
        }
    }
}
