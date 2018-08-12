using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chronicle.Persistance
{
    internal interface ISagaLog
    {
        Task SaveAsync(ISagaLogData message);
        Task<IEnumerable<ISagaLogData>> GetAsync(Guid sagaId);
    }
}
