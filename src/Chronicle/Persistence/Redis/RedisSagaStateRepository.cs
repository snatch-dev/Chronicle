using System;
using System.Threading.Tasks;
using Chronicle.Utils;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Chronicle.Persistence
{
    public class RedisSagaStateRepository : ISagaStateRepository
    {
        private readonly IDistributedCache _cache;

        public RedisSagaStateRepository(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<ISagaState> ReadAsync(SagaId sagaId, Type sagaType, Type dataType = null)
        {
            if (String.IsNullOrWhiteSpace(sagaId))
            {
                throw new ArgumentNullException(nameof(sagaId));
            }
            if (sagaType is null)
            {
                throw new ArgumentNullException(nameof(sagaType));
            }
            if (dataType is null)
            {
                throw new ArgumentNullException(nameof(dataType));
            }

            ISagaState state = null;
            var cachedSagaState = await _cache.GetStringAsync(StateId(sagaId, sagaType));
            if (!String.IsNullOrWhiteSpace(cachedSagaState))
            {
                state = JsonConvert.DeserializeObject<SagaState>(cachedSagaState);
                if (dataType != null)
                {
                    state.Update(state.State, (state.Data as JObject).ToObject(dataType));
                }
            }
            return state;
        }

        public async Task WriteAsync(ISagaState state)
        {
            if (state is null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            var sagaStateString = JsonConvert.SerializeObject(state);
            await _cache.SetStringAsync(StateId(state.SagaId, state.SagaType), sagaStateString);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(SagaId sagaId, Type sagaType)
        {
            if (String.IsNullOrWhiteSpace(sagaId))
            {
                throw new ArgumentException(nameof(sagaId));
            }
            if (sagaType is null)
            {
                throw new ArgumentException(nameof(sagaType));
            }
            await _cache.RemoveAsync(StateId(sagaId, sagaType));
            await Task.CompletedTask;
        }

        private string StateId(string id, Type type) => $"_state_{id}_{type.GetHashCode()}";
    }
}