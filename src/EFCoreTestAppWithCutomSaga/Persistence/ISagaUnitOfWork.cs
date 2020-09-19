using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EFCoreTestApp.Persistence
{
    public interface IUnitOfWork
    {
        Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken));

        // Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default(CancellationToken));
    }

    public interface ITransaction : IDisposable
    {
        void Commit();

        void Rollback();
    }

    public interface ISagaUnitOfWork : IUnitOfWork
    {
        ISagaLogRepository SagaLogRepository { get; }
        ISagaStateDBRepository SagaStateDBRepository { get; }
    }
}
