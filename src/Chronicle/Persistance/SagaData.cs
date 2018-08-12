using System;

namespace Chronicle.Persistance
{
    internal class SagaData<TData> : ISagaData<TData> where TData : class
    {
        public Guid SagaId { get; }

        public SagaStates State { get; }

        public TData Data { get; }

        private SagaData(Guid sagaId, SagaStates state, TData data)
            => (SagaId, State, Data) = (sagaId, state, data);

        public static ISagaData<TData> Create(Guid sagaId, SagaStates state, TData data)
            => new SagaData<TData>(sagaId, state, data);
    }
}
