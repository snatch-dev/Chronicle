using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EFCoreTestApp.Persistence
{
    public class SagaUnitOfWork<TContext> : ICustomUnitOfWork<TContext> where TContext : DbContext
    {
        protected readonly TContext DbContext;

        public SagaUnitOfWork(TContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }

    }
}
