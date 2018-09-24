using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chronicle.Persistence
{
    internal class InMememorySagaLog : ISagaLog
    {
        private readonly List<ISagaLogData> _sagaLog;

        public InMememorySagaLog()
            => _sagaLog = new List<ISagaLogData>();

        public async Task<IEnumerable<ISagaLogData>> GetAsync(Guid sagaId, Type sagaType)
            => await Task.FromResult(_sagaLog.Where(sld => sld.SagaId == sagaId && sld.SagaType == sagaType));

        public async Task SaveAsync(ISagaLogData message)
        {
            _sagaLog.Add(message);
            await Task.CompletedTask;
        }
    }
}
