using System;
using Microsoft.EntityFrameworkCore;
using Chronicle.Integrations.EFCore.Repositories;

namespace EFCoreTestApp.Persistence
{
    // Inhert from ISagaUnitOfWork from Chronicle.Integrations.EFCore.Repositories
    public interface ICustomUnitOfWork<TContext> : ISagaUnitOfWork<TContext> where TContext : DbContext
    {
    }
}
