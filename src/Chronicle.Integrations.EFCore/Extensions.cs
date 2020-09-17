using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Chronicle.Integrations.EFCore.Repositories;
using Chronicle.Integrations.EFCore.Persistence;

namespace Chronicle.Integrations.EFCore
{
    public static class Extensions
    {
        public static IChronicleBuilder UseEFCorePersistence(this IChronicleBuilder builder, string dbConnectionString)
        {
            builder.Services.AddTransient<ISagaLogRepository, SagaLogRepository>();
            builder.Services.AddTransient<ISagaStateDBRepository, SagaStateRepository>();
            builder.Services.AddDbContext<SagaDbContext>(builder =>
            {
                builder.UseSqlServer(dbConnectionString);
            }, ServiceLifetime.Transient);
            builder.UseSagaLog<EFCoreSagaLog>();
            builder.UseSagaStateRepository<EFCoreSagaState>();

            return builder;
        }
    }
}
