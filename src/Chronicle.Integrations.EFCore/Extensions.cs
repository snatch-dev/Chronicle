using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Chronicle.Integrations.EFCore.Repositories;
using Chronicle.Integrations.EFCore.Persistence;

namespace Chronicle.Integrations.EFCore
{
    public static class Extensions
    {
        public static IChronicleBuilder UseEFCorePersistence<TContext>(this IChronicleBuilder builder)
               where TContext: DbContext
        {
            builder.Services.AddTransient<ISagaLogRepository, SagaLogRepository<TContext>>();
            builder.Services.AddTransient<ISagaStateDBRepository, SagaStateRepository<TContext>>();
            builder.UseSagaLog<EFCoreSagaLog>();
            builder.UseSagaStateRepository<EFCoreSagaState>();

            return builder;
        }
    }
}
