using System;
using System.Reflection;
using MongoDB.Bson.Serialization.Attributes;

namespace Chronicle.Integrations.MongoDB.Persistence
{
    internal class MongoSagaState : ISagaState
    {
        [BsonId]
        [BsonElement("Id")]
        public string MongoId { get; set; }
        [BsonIgnore]
        public SagaId Id => MongoId;

        public string SagaType { get; set; }
        public SagaStates State { get; set; }
        public object Data { get; set; }
        Type ISagaState.Type => Assembly.GetEntryAssembly()?.GetType(SagaType);

        public void Update(SagaStates state, object data = null)
        {
            State = state;
            Data = data;
        }
    }
}
