using System;

namespace Chronicle
{
    public interface ISagaContextBuilder
    {
        ISagaContextBuilder WithCorrelationId(Guid correlationId);
        ISagaContextBuilder WithOriginator(string originator);
        ISagaContextBuilder WithMetadata(string key, object value);
        ISagaContext Build();
    }
}
