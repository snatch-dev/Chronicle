using System;
using Newtonsoft.Json;

namespace Chronicle
{
    public struct SagaId
    {
        public string Id { get; }

        [JsonConstructor]
        public SagaId(string id) => Id = id;

        public static implicit operator string(SagaId sagaId) => sagaId.Id;

        public static implicit operator SagaId(string sagaId) => new SagaId(sagaId);

        public static SagaId NewSagaId() => new SagaId(Guid.NewGuid().ToString());

        public override string ToString() => Id;
    }
}