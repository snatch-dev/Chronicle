using System;

namespace Chronicle
{
    public interface ISagaState
    {
        SagaId SagaId { get; }
        Type SagaType { get; }
        SagaStates State { get; }
        object Data { get; }
        void Update(SagaStates state, object data = null);
    }
}