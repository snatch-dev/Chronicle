using Chronicle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EFCoreTestApp.SagaRepository;

namespace EFCoreTestApp.Persistence
{
    public interface ISagaStateDBRepository
    {
        Task<EFCoreSagaStateData> ReadAsync(SagaId id, Type type);
        Task WriteAsync(EFCoreSagaStateData message);

        Task<EFCoreSagaStateData> GetByIdAsync(SagaId id);
    }
}
