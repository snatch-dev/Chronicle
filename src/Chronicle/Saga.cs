using System;

namespace Chronicle
{
    public abstract class Saga : ISaga
    {
        public Guid Id { get; private set; }

        public SagaStates State { get; protected set; }

        public virtual void Initialize(Guid id, SagaStates state)
            => (Id, State) = (id, state);

        public virtual Guid ResolveId(object message, ISagaContext context)
            => context.CorrelationId;

        public virtual void Complete()
            => State = SagaStates.Completed;

        public virtual void Reject()
            => State = SagaStates.Rejected;
    }

    public abstract class Saga<TData> : Saga, ISaga<TData> where TData : class, new()
    {
        public TData Data { get; protected set; }

        public virtual void Initialize(Guid id, SagaStates state, TData data)
        {
            base.Initialize(id, state);
            Data = data;
        }
    }
}
