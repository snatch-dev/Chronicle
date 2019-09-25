using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chronicle.Persistence
{
    internal class InMemorySagaStateRepository : ISagaStateRepository
    {
        private readonly List<ISagaState> _repository;

        public InMemorySagaStateRepository() => _repository = new List<ISagaState>();

        public Task<ISagaState> ReadAsync(SagaId id, Type type)
            => Task.FromResult(_repository.FirstOrDefault(s => s.SagaId == id && s.SagaType == type));

        public async Task WriteAsync(ISagaState state)
        {
            var sagaDataToUpdate = await ReadAsync(state.SagaId, state.SagaType);

            _repository.Remove(sagaDataToUpdate);
            _repository.Add(state);

            await Task.CompletedTask;
        }
    }
}