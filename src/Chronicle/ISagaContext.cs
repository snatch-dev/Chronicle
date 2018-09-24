using System;

namespace Chronicle
{
    public interface ISagaContext
    {
        Guid CorrelationId { get; }
        string Originator { get; }
    }
}
