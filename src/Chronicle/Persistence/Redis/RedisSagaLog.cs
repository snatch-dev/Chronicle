using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chronicle;
using Chronicle.Utils;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Chronicle.Persistence
{
    public class RedisSagaLog : ISagaLog
    {
        private readonly IDistributedCache _cache;

        public RedisSagaLog(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<IEnumerable<ISagaLogData>> ReadAsync(SagaId id, Type sagaType)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException(nameof(id));
            }
            if (sagaType is null)
            {
                throw new ArgumentException(nameof(sagaType));
            }
            List<SagaLogData> sagaLogDatas = new List<SagaLogData>();
            IEnumerable<SagaLogData> serializedSagaLogDatas = new List<SagaLogData>();
            var cachedSagaLogDatasString = await _cache.GetStringAsync(LogId(id, sagaType));
            if (!String.IsNullOrWhiteSpace(cachedSagaLogDatasString))
            {
                sagaLogDatas = JsonConvert.DeserializeObject<List<SagaLogData>>(cachedSagaLogDatasString);
                serializedSagaLogDatas = sagaLogDatas.Select(s =>
                {
                    var data = (s.Message as JObject).ToObject(s.MessageType);
                    return new SagaLogData(s.SagaId, s.SagaType, s.CreatedAt, s.Message, s.MessageType);
                });
            }
            return serializedSagaLogDatas;
        }

        public async Task WriteAsync(ISagaLogData logData)
        {
            if (logData is null)
            {
                throw new ArgumentException(nameof(logData));
            }
            IList<ISagaLogData> sagaLogDatas = (await ReadAsync(logData.SagaId, logData.SagaType)).ToList();
            sagaLogDatas.Add(logData);

            var sagaLogDatasString = JsonConvert.SerializeObject(sagaLogDatas);
            await _cache.SetStringAsync(LogId(logData.SagaId, logData.SagaType), sagaLogDatasString);

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

            await _cache.RemoveAsync(LogId(sagaId, sagaType));
            await Task.CompletedTask;
        }

        private string LogId(string id, Type type) => $"_log_{id}_{type.GetHashCode()}";
    }
}