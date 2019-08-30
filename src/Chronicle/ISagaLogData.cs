using System;

namespace Chronicle
{
    public interface ISagaLogData
    {
        SagaId SagaId { get; }
        Type SagaType { get; }
        long CreatedAt { get; }
        object Message { get; }
    }
}