using System;
using Chronicle.Utils;

namespace Chronicle.Persistence
{
    internal class SagaLogData : ISagaLogData
    {
        public SagaId SagaId { get; }
        public Type SagaType { get; }
        public long CreatedAt { get; }
        public object Message { get; }

        private SagaLogData(SagaId sagaId, Type sagaType, long createdAt, object message)
            => (SagaId, SagaType, CreatedAt, Message) = (sagaId, sagaType, createdAt, message);

        public static ISagaLogData Create(SagaId sagaId, Type sagaType, object message)
            => new SagaLogData(sagaId, sagaType, DateTimeOffset.Now.GetTimeStamp(), message);
    }
}
