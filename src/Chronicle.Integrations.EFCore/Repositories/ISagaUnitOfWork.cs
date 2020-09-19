using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Chronicle.Integrations.EFCore.Repositories
{
    public interface ISagaUnitOfWork<TContext> where TContext: DbContext
    {
        Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
