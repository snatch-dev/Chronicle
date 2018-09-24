using System;

namespace Chronicle
{
    public interface ISagaLogData
    {
        Guid SagaId { get; }
        Type SagaType { get; }
        long CreatedAt { get; }
        object Message { get; }
    }
}
