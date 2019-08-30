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

        public Task<ISagaState> ReadAsync(SagaId sagaId, Type sagaType, Type dataType = null) => Task.FromResult(_repository.FirstOrDefault(s => s.SagaId == sagaId && s.SagaType == sagaType));

        public async Task WriteAsync(ISagaState state)
        {
            var sagaDataToUpdate = await ReadAsync(state.SagaId, state.SagaType);

            _repository.Remove(sagaDataToUpdate);
            _repository.Add(state);

            await Task.CompletedTask;
        }

        public async Task DeleteAsync(SagaId sagaId, Type sagaType)
        {
            _repository.RemoveAll(s => s.SagaId == sagaId && s.SagaType == sagaType);
            await Task.CompletedTask;
        }
    }
}