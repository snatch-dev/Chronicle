using System;
using System.Threading.Tasks;
using Chronicle.Persistence;
using Chronicle.Sagas;

namespace Chronicle
{
    public abstract class Saga<TData> : ISaga<TData> where TData : class
    {
        public Guid Id { get; private set; }

        public SagaStates State { get; protected set; }

        public TData Data { get; protected set; }

        public virtual void Initialize(Guid id, SagaStates state, TData data)
            => (Id, State, Data) = (id, state, data);

        public virtual void Complete()
            => State = SagaStates.Completed;

        public virtual void Reject()
            => State = SagaStates.Rejected;
    }
}
