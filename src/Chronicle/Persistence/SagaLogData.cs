using System;
using Chronicle.Utils;

namespace Chronicle.Persistence
{
    internal class SagaLogData : ISagaLogData
    {
        public SagaId Id { get; }
        public Type Type { get; }
        public long CreatedAt { get; }
        public object Message { get; }

        private SagaLogData(SagaId sagaId, Type sagaType, long createdAt, object message) 
            => (Id, Type, CreatedAt, Message) = (sagaId, sagaType, createdAt, message);

        public static ISagaLogData Create(SagaId sagaId, Type sagaType, object message) 
            => new SagaLogData(sagaId, sagaType, DateTimeOffset.Now.GetTimeStamp(), message);
    }
}
