using System;
using System.Reflection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chronicle.Integrations.MongoDB.Persistence
{
    internal class MongoSagaLogData : ISagaLogData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string MongoId { get; set; }
        public string SagaId { get; set; }
        [BsonIgnore]
        public SagaId Id => SagaId;
        public string SagaType { get; set; }
        public long CreatedAt { get; set; }
        public object Message { get; set; }
        Type ISagaLogData.Type => Assembly.GetEntryAssembly()?.GetType(SagaType);
    }
}
