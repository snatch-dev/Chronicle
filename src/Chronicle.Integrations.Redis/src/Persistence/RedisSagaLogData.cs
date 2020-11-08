using System;
using Newtonsoft.Json;

namespace Chronicle.Integrations.Redis.Persistence
{
    internal sealed class RedisSagaLogData : ISagaLogData
    {
        public string SagaId { get; set; }
        [JsonIgnore]
        public SagaId Id => SagaId;
        public Type Type { get; set; }
        public long CreatedAt { get; set; }
        public object Message { get; set; }
        public Type MessageType { get; set; }

    }
}