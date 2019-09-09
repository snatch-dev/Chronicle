using System;

namespace Chronicle.Persistence
{
    internal class SagaState : ISagaState
    {
        public SagaId SagaId { get; }
        public Type SagaType { get; }
        public SagaStates State { get; private set; }
        public object Data { get; private set; }

        private SagaState(SagaId id, Type type, SagaStates state, object data)
            => (SagaId, SagaType, State, Data) = (id, type, state, data);

        public static ISagaState Create(SagaId id, Type type, SagaStates state, object data = null)
            => new SagaState(id, type, state, data);

        public void Update(SagaStates state, object data = null)
        {
            State = state;
            Data = data;
        }
    }
}
