using Chronicle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EFCoreTestApp.SagaRepository;

namespace EFCoreTestApp.Persistence
{
    public interface ISagaLogRepository
    {
        Task<IReadOnlyCollection<EFCoreSagaLogData>> ReadAsync(SagaId id, Type type);

        Task<IReadOnlyCollection<EFCoreSagaLogData>> ReadByIdAsync(SagaId id);

        Task WriteAsync(EFCoreSagaLogData message);
    }
}
