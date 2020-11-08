using System;
using Newtonsoft.Json;

namespace Chronicle.Integrations.Redis.Persistence
{
    internal sealed class RedisSagaState : ISagaState
    {
        public string SagaId { get; set; }
        [JsonIgnore]
        public SagaId Id => SagaId;
        public Type Type { get; set; }
        public SagaStates State { get; set; }
        public object Data { get; set; }
        public Type DataType { get; set; }

        public void Update(SagaStates state, object data = null)
        {
            State = state;
            Data = data;
        }
    }
}