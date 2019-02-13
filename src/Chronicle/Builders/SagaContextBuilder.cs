using System;
using System.Collections.Generic;
using Chronicle.Persistence;

namespace Chronicle.Builders
{
    internal sealed class SagaContextBuilder : ISagaContextBuilder
    {
        private Guid _correlationId;
        private string _originator;
        private readonly List<ISagaContextMetadata> _metadata;

        public SagaContextBuilder()
            => _metadata = new List<ISagaContextMetadata>();

        public ISagaContextBuilder WithCorrelationId(Guid correlationId)
        {
            _correlationId = correlationId;
            return this;
        }

        public ISagaContextBuilder WithOriginator(string originator)
        {
            _originator = originator;
            return this;
        }
        
        public ISagaContextBuilder WithMetadata(string key, object value)
        {
            var metadata = new SagaContextMetadata(key, value);
            _metadata.Add(metadata);
            return this;
        }

        public ISagaContext Build()
            => SagaContext.Create(_correlationId, _originator, _metadata);
    }
}
