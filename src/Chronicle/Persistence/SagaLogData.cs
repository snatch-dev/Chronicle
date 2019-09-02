using System;
using Chronicle.Utils;
using Newtonsoft.Json;

namespace Chronicle.Persistence
{
    public class SagaLogData : ISagaLogData
    {
        public SagaId SagaId { get; }
        public Type SagaType { get; }
        public long CreatedAt { get; }
        public object Message { get; }
        public Type MessageType { get; }

        [JsonConstructor]
        public SagaLogData(SagaId sagaId, Type sagaType, long createdAt, object message, Type messageType) => (SagaId, SagaType, CreatedAt, Message, MessageType) = (sagaId, sagaType, createdAt, message, messageType);

        public static ISagaLogData Create(SagaId sagaId, Type sagaType, object message, Type messageType) => new SagaLogData(sagaId, sagaType, DateTimeOffset.Now.GetTimeStamp(), message, messageType);
    }
}