using System;

namespace Chronicle
{
    public interface ISagaState
    {
        Guid Id { get; }
        Type Type { get; }
        SagaStates State { get; }
        object Data { get; }
        void Update(SagaStates state, object data = null);
    }
}
