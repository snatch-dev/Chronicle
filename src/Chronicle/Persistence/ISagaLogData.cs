using System;

namespace Chronicle.Persistence
{
    internal interface ISagaLogData
    {
        Guid SagaId { get; }
        long CreatedAt { get; }
        object Message { get; }
    }
}
