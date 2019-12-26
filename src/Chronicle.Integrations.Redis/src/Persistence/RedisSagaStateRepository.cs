using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Chronicle.Integrations.Redis.Persistence
{
    internal sealed class RedisSagaStateRepository : ISagaStateRepository
    {
        private readonly IDistributedCache _cache;

        public RedisSagaStateRepository(IDistributedCache cache)
            => _cache = cache;
        
        public async Task<ISagaState> ReadAsync(SagaId sagaId, Type sagaType)
        {
            if (string.IsNullOrWhiteSpace(sagaId))
            {
                throw new ChronicleException($"{nameof(sagaId)} was null or whitespace.");
            }
            if (sagaType is null)
            {
                throw new ChronicleException($"{nameof(sagaType)} was null.");
            }

            RedisSagaState state = null;
            var cachedSagaState = await _cache.GetStringAsync(StateId(sagaId, sagaType));
            
            if (!string.IsNullOrWhiteSpace(cachedSagaState))
            {
                state = JsonConvert.DeserializeObject<RedisSagaState>(cachedSagaState);
                state.Update(state.State, (state.Data as JObject)?.ToObject(state.DataType));
            }
            return state;
        }

        public async Task WriteAsync(ISagaState state)
        {
            if (state is null)
            {
                throw new ChronicleException($"{nameof(state)} was null.");
            }

            var sagaState = new RedisSagaState(state.Id, state.Type, state.State, state.Data, state.Data.GetType());


            var serializedSagaState = JsonConvert.SerializeObject(sagaState);
            await _cache.SetStringAsync(StateId(state.Id, state.Type), serializedSagaState);
        }

        public async Task DeleteAsync(SagaId sagaId, Type sagaType)
        {
            if (string.IsNullOrWhiteSpace(sagaId))
            {
                throw new ChronicleException($"{nameof(sagaId)} was null or whitespace.");
            }
            if (sagaType is null)
            {
                throw new ChronicleException($"{nameof(sagaType)} was null.");
            }
            await _cache.RemoveAsync(StateId(sagaId, sagaType));
        }

        private string StateId(string id, Type type) => $"_state_{id}_{type.GetHashCode()}";
    }
}