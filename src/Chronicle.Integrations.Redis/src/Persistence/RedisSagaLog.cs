using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Chronicle.Integrations.Redis.Persistence
{
    internal sealed class RedisSagaLog : ISagaLog
    {
        private readonly IDistributedCache cache;

        public RedisSagaLog(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public async Task<IEnumerable<ISagaLogData>> ReadAsync(SagaId id, Type sagaType)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ChronicleException($"{nameof(id)} was null.");
            }
            if (sagaType is null)
            {
                throw new ChronicleException($"{nameof(sagaType)} was null.");
            }

            var sagaLogDatas = new List<RedisSagaLogData>();
            var deserializedSagaLogDatas = new List<RedisSagaLogData>();
            var cachedSagaLogDatas = await cache.GetStringAsync(LogId(id, sagaType));
            
            if (!string.IsNullOrWhiteSpace(cachedSagaLogDatas))
            {
                sagaLogDatas = JsonConvert.DeserializeObject<List<RedisSagaLogData>>(cachedSagaLogDatas);
                sagaLogDatas.ForEach(sld =>
                {
                    {
                        var message = (sld.Message as JObject)?.ToObject(sld.MessageType);
                        deserializedSagaLogDatas.Add(new RedisSagaLogData(sld.Id, sld.Type, sld.CreatedAt, message, sld.MessageType));
                    }
                });
            }
            return deserializedSagaLogDatas;
        }

        public async Task WriteAsync(ISagaLogData logData)
        {
            if (logData is null)
            {
                throw new ChronicleException($"{nameof(logData)} was null.");
            }
            var sagaLogDatas = (await ReadAsync(logData.Id, logData.Type)).ToList();

            var sagaLogData = new RedisSagaLogData(logData.Id, logData.Type, logData.CreatedAt, logData.Message, logData.Message.GetType());

            sagaLogDatas.Add(sagaLogData);

            var serializedSagaLogDatas = JsonConvert.SerializeObject(sagaLogDatas);
            await cache.SetStringAsync(LogId(logData.Id, logData.Type), serializedSagaLogDatas);
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

            await cache.RemoveAsync(LogId(sagaId, sagaType));
        }

        private string LogId(string id, Type type) => $"_log_{id}_{type.GetHashCode()}";
    }
}
