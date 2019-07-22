using System.Collections.Generic;
using System.Linq;
using Chronicle.Builders;

namespace Chronicle
{
    public sealed class SagaContext : ISagaContext
    {
        public SagaId SagaId { get; }
        public string Originator { get; }
        public IReadOnlyCollection<ISagaContextMetadata> Metadata { get; }
        public SagaContextError SagaContextError { get; set; }

        private SagaContext(SagaId sagaId, string originator, IEnumerable<ISagaContextMetadata> metadata)
        {
            SagaId = sagaId;
            Originator = originator;

            var areMetadataKeysUnique = metadata.GroupBy(m => m.Key).All(g => g.Count() is 1);

            if (!areMetadataKeysUnique)
            {
                throw new ChronicleException("Metadata keys are not unique");
            }

            Metadata = metadata.ToList().AsReadOnly();
        }

        public static ISagaContext Empty =>
            new SagaContext(SagaId.NewSagaId(), string.Empty, Enumerable.Empty<ISagaContextMetadata>());


        public static ISagaContext Create(SagaId sagaId, string originator, IEnumerable<ISagaContextMetadata> metadata)
            => new SagaContext(sagaId, originator, metadata);

        public static ISagaContextBuilder Create() 
            => new SagaContextBuilder();

        public ISagaContextMetadata GetMetadata(string key) 
            => Metadata.Single(m => m.Key == key);

        public bool TryGetMetadata(string key, out ISagaContextMetadata metadata)
        {
            metadata = Metadata.SingleOrDefault(m => m.Key == key);
            return metadata != null;
        }
    }
}
