using System;

namespace Chronicle
{
    public interface ISagaLogData
    {
        Guid Id { get; }
        Type Type { get; }
        long CreatedAt { get; }
        object Message { get; }
    }
}
