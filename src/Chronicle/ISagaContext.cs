using System;
using System.Collections.Generic;

namespace Chronicle
{
    public interface ISagaContext
    {
        Guid CorrelationId { get; }
        string Originator { get; }
        IReadOnlyCollection<ISagaContextMetadata> Metadata { get; }
        ISagaContextMetadata GetMetadata(string key);
        bool TryGetMetadata(string key, out ISagaContextMetadata metadata);
    }
}
