using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chronicle.Integrations.EFCore.Persistence;

namespace Chronicle.Integrations.EFCore.Repositories
{
    internal interface ISagaLogRepository
    {
        Task<IReadOnlyCollection<EFCoreSagaLogData>> ReadAsync(SagaId id, Type type);

        Task WriteAsync(EFCoreSagaLogData message);
    }
}
