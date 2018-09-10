using System;

namespace Chronicle
{
    public abstract class Saga<TData> : ISaga<TData> where TData : class, new()
    {
        public Guid Id { get; private set; }

        public SagaStates State { get; protected set; }

        public TData Data { get; protected set; }

        object ISaga.Data => Data;

        public virtual void Initialize(Guid id, SagaStates state, object data)
            => (Id, State, Data) = (id, state, (TData) data);

        public virtual void Complete()
            => State = SagaStates.Completed;

        public virtual void Reject()
            => State = SagaStates.Rejected;
    }
}
