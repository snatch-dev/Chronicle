using System;

namespace Chronicle.Persistence
{
    internal class SagaData : ISagaData
    {
        public Guid SagaId { get; }
        public Type SagaType { get; }
        public SagaStates State { get; private set; }
        public object Data { get; private set; }

        private SagaData(Guid sagaId, Type sagaType, SagaStates state, object data)
            => (SagaId, SagaType, State, Data) = (sagaId, sagaType, state, data);

        public static ISagaData Create(Guid sagaId, Type sagaType, SagaStates state, object data = null)
            => new SagaData(sagaId, sagaType, state, data);
    }
}
