using System;

namespace Chronicle
{
    public interface ISagaData
    {
        Guid SagaId { get; }
        Type SagaType { get; }
        SagaStates State { get; }
        object Data { get; }
    }
}
