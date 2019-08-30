using System;
using Newtonsoft.Json;

namespace Chronicle.Persistence
{
    internal class SagaState : ISagaState
    {
        public SagaId SagaId { get; }
        public Type SagaType { get; }
        public SagaStates State { get; private set; }
        public object Data { get; private set; }

        [JsonConstructor]
        public SagaState(SagaId sagaId, Type sagaType, SagaStates state, object data) => (SagaId, SagaType, State, Data) = (sagaId, sagaType, state, data);

        public static ISagaState Create(SagaId sagaId, Type sagaType, SagaStates state, object data = null) => new SagaState(sagaId, sagaType, state, data);

        public void Update(SagaStates state, object data = null)
        {
            State = state;
            Data = data;
        }
    }
}