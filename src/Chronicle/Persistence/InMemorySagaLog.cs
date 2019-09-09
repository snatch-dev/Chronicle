using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chronicle.Persistence
{
    internal class InMemorySagaLog : ISagaLog
    {
        private readonly List<ISagaLogData> _sagaLog;

        public InMemorySagaLog()
            => _sagaLog = new List<ISagaLogData>();

        public Task DeleteAsync(SagaId sagaId, Type sagaType)
            => Task.FromResult(_sagaLog.RemoveAll(sld => sld.SagaId == sagaId && sld.SagaType == sagaType));

        public Task<IEnumerable<ISagaLogData>> ReadAsync(SagaId id, Type type)
            => Task.FromResult(_sagaLog.Where(sld => sld.SagaId == id && sld.SagaType == type));

        public async Task WriteAsync(ISagaLogData message)
        {
            _sagaLog.Add(message);
            await Task.CompletedTask;
        }
    }
}
