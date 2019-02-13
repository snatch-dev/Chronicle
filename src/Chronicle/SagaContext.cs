using System;
using System.Collections.Generic;
using System.Linq;
using Chronicle.Builders;

namespace Chronicle
{
    public class SagaContext : ISagaContext
    {
        public Guid CorrelationId { get; }

        public string Originator { get; }
        public IReadOnlyCollection<ISagaContextMetadata> Metadata { get; }

        public static readonly ISagaContextBuilder Builder = new SagaContextBuilder();

        private SagaContext(Guid correlationId, string originator, IEnumerable<ISagaContextMetadata> metadata)
        {
            CorrelationId = correlationId;
            Originator = originator;

            var areMetadataKeysUnique = metadata.GroupBy(m => m.Key).All(g => g.Count() == 1);

            if (!areMetadataKeysUnique)
            {
                throw new ChronicleException("Metadata keys are not unique");
            }

            Metadata = metadata.ToList().AsReadOnly();
        }

        public static ISagaContext Empty
            => new SagaContext(Guid.NewGuid(), string.Empty, Enumerable.Empty<ISagaContextMetadata>());

        public static ISagaContext Create(Guid correlationId, string originator, IEnumerable<ISagaContextMetadata> metadata)
            => new SagaContext(correlationId, originator, metadata);
        
        public ISagaContextMetadata GetMetadata(string key)
            => Metadata.Single(m => m.Key == key);

        public bool TryGetMetadata(string key, out ISagaContextMetadata metadata)
        {
            metadata = Metadata.SingleOrDefault(m => m.Key == key);
            return metadata != null;
        }
    }
}
