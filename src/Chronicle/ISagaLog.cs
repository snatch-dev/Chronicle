using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chronicle
{
    public interface ISagaLog
    {
        Task<IEnumerable<ISagaLogData>> ReadAsync(Guid id, Type type);
        Task WriteAsync(ISagaLogData message);
    }
}
