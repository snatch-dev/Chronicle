using System;

namespace Chronicle.Persistance
{
    internal interface ISagaLogData
    {
        Guid SagaId { get; }
        long CreatedAt { get; }
        object Message { get; }
    }
}
