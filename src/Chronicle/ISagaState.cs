using System;

namespace Chronicle
{
    public interface ISagaState
    {
        SagaId Id { get; }
        Type Type { get; }
        SagaStates State { get; }
        object Data { get; }
        void Update(SagaStates state, object data = null);
    }
}
