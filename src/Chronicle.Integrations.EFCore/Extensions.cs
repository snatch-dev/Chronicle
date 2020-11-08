using Microsoft.EntityFrameworkCore;
using Chronicle.Integrations.EFCore.Persistence;

namespace Chronicle.Integrations.EFCore
{
    public static class Extensions
    {
        public static IChronicleBuilder UseEFCorePersistence<TContext>(this IChronicleBuilder builder)
               where TContext: DbContext
        {
            builder.UseSagaLog<EFCoreSagaLog<TContext>>();
            builder.UseSagaStateRepository<EFCoreSagaState<TContext>>();

            return builder;
        }
    }
}
