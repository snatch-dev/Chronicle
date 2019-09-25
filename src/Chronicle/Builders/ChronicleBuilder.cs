using System;
using Chronicle.Errors;
using Chronicle.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Chronicle.Builders
{
    internal class ChronicleBuilder : IChronicleBuilder
    {
        public IServiceCollection Services { get; }
        public IChronicleConfig Config { get; }

        public ChronicleBuilder(IServiceCollection services)
        {
            Services = services;
            Config = new ChronicleConfig();
            services.AddSingleton<IChronicleConfig>(Config);
        }

        public IChronicleBuilder UseInMemoryPersistence()
        {
            Services.AddSingleton(typeof(ISagaStateRepository), typeof(InMemorySagaStateRepository));
            Services.AddSingleton(typeof(ISagaLog), typeof(InMemorySagaLog));
            return this;
        }

        public IChronicleBuilder UseSagaLog<TSagaLog>() where TSagaLog : ISagaLog
        {
            Services.AddTransient(typeof(ISagaLog), typeof(TSagaLog));
            return this;
        }

        public IChronicleBuilder UseSagaStateRepository<TRepository>() where TRepository : ISagaStateRepository
        {
            Services.AddTransient(typeof(ISagaStateRepository), typeof(TRepository));
            return this;
        }
    }
}
