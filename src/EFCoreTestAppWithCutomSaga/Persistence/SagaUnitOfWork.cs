using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EFCoreTestApp.Persistence
{
    public class SagaUnitOfWork: ISagaUnitOfWork, IUnitOfWork
    {
        protected readonly SagaDbContext DbContext;
        public ISagaLogRepository SagaLogRepository { get; }
        public ISagaStateDBRepository SagaStateDBRepository { get; }

        public SagaUnitOfWork(SagaDbContext dbContext, ISagaLogRepository _sagaLogRepository, ISagaStateDBRepository _sagaStateDBRepository)
        {
            DbContext = dbContext;
            SagaLogRepository = _sagaLogRepository;
            SagaStateDBRepository = _sagaStateDBRepository;
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }

    }
}
