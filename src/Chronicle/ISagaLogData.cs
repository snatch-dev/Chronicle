using System;

namespace Chronicle
{
    public interface ISagaLogData
    {
        Guid SagaId { get; }
        long CreatedAt { get; }
        object Message { get; }
    }
}
