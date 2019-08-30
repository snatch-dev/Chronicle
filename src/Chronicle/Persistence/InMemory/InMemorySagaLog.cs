using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chronicle.Persistence
{
    internal class InMemorySagaLog : ISagaLog
    {
        private readonly List<ISagaLogData> _sagaLog;

        public InMemorySagaLog() => _sagaLog = new List<ISagaLogData>();

        public Task<IEnumerable<ISagaLogData>> ReadAsync(SagaId sagaId, Type sagaType) => Task.FromResult(_sagaLog.Where(sld => sld.SagaId == sagaId && sld.SagaType == sagaType));

        public async Task WriteAsync(ISagaLogData message)
        {
            _sagaLog.Add(message);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(SagaId sagaId, Type sagaType)
        {
            _sagaLog.RemoveAll(sld => sld.SagaId == sagaId && sld.SagaType == sagaType);
            await Task.CompletedTask;
        }
    }
}