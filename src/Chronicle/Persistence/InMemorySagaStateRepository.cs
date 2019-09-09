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

        public Task DeleteAsync(SagaId sagaId, Type sagaType)
            => Task.FromResult(_repository.RemoveAll(s => s.Id == sagaId && s.Type == sagaType));

        public Task<ISagaState> ReadAsync(SagaId id, Type type)
            => Task.FromResult(_repository.FirstOrDefault(s => s.Id == id && s.Type == type));

        public async Task WriteAsync(ISagaState state)
        {
            var sagaDataToUpdate = await ReadAsync(state.Id, state.Type);

            _repository.Remove(sagaDataToUpdate);
            _repository.Add(state);

            await Task.CompletedTask;
        }
    }
}
