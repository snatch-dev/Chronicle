using System;
using Chronicle.Utils;

namespace Chronicle.Persistence
{
    internal class SagaLogData : ISagaLogData
    {
        public Guid SagaId { get; }

        public long CreatedAt { get; }

        public object Message { get; }

        private SagaLogData(Guid sagaId, long createdAt, object message)
            => (SagaId, CreatedAt, Message) = (sagaId, createdAt, message);

        public static ISagaLogData Create(Guid sagaId, object message)
            => new SagaLogData(sagaId, DateTimeOffset.Now.GetTimeStamp(), message);
    }
}
