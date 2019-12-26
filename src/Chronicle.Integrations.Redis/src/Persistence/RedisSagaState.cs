using System;
using Newtonsoft.Json;

namespace Chronicle.Integrations.Redis.Persistence
{
    internal sealed class RedisSagaState : ISagaState
    {
        public SagaId Id { get; }
        public Type Type { get; }
        public SagaStates State { get; private set; }
        public object Data { get; private set; }
        public Type DataType { get; }

        [JsonConstructor]
        public RedisSagaState(SagaId id, Type type, SagaStates state, object data = null, Type dataType = null)
            => (Id, Type, State, Data, DataType) = (id, type, state, data, dataType);

        public static ISagaState Create(SagaId sagaId, Type sagaType, SagaStates state, object data = null, Type dataType = null)
            => new RedisSagaState(sagaId, sagaType, state, data, dataType);

        public void Update(SagaStates state, object data = null)
        {
            State = state;
            Data = data;
        }
    }
}