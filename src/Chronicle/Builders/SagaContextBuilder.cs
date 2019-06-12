using System.Collections.Generic;
using Chronicle.Persistence;

namespace Chronicle.Builders
{
    internal sealed class SagaContextBuilder : ISagaContextBuilder
    {
        private SagaId _sagaId;
        private string _originator;
        private readonly List<ISagaContextMetadata> _metadata;

        public SagaContextBuilder()
            => _metadata = new List<ISagaContextMetadata>();

        public ISagaContextBuilder WithSagaId(SagaId sagaId)
        {
            _sagaId = sagaId;
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

        public ISagaContextBuilder WithMetadata(ISagaContextMetadata sagaContextMetadata)
        {
            _metadata.Add(sagaContextMetadata);
            return this;
        }

        public ISagaContext Build()
            => SagaContext.Create(_sagaId, _originator, _metadata);
    }
}
