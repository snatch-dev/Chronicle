using System;

namespace Chronicle
{
    public interface ISagaLogData
    {
        SagaId Id { get; }
        Type Type { get; }
        long CreatedAt { get; }
        object Message { get; }
    }
}
