using System;
using Chronicle.Errors;
using Chronicle.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Chronicle.Builders
{
    internal class ChronicleBuilder : IChronicleBuilder
    {
        public IServiceCollection Services { get; }

        public ChronicleBuilder(IServiceCollection services)
            => Services = services;

        public IChronicleBuilder UseInMemoryPersistence()
        {
            Services.AddSingleton(typeof(ISagaDataRepository), typeof(InMemorySagaDataRepository));
            Services.AddSingleton(typeof(ISagaLog), typeof(InMemorySagaLog));
            return this;
        }

        public IChronicleBuilder UseSagaLog<TSagaLog>() where TSagaLog : ISagaLog
            => UseSagaLog(typeof(TSagaLog));

        public IChronicleBuilder UseSagaLog(Type sagaLogType)
        {
            Check.Is<ISagaLog>(sagaLogType, ChronicleBuilderErrorMessages.InvalidSagaLogType);
            Services.AddTransient(typeof(ISagaLog), sagaLogType);
            return this;
        }

        public IChronicleBuilder UseSagaDataRepository<TRepository>() where TRepository : ISagaDataRepository
            => UseSagaDataRepository(typeof(TRepository));

        public IChronicleBuilder UseSagaDataRepository(Type repositoryType)
        {
            Check.Is<ISagaDataRepository>(repositoryType, ChronicleBuilderErrorMessages.InvalidSagaDataRepositoryType);
            Services.AddTransient(typeof(ISagaDataRepository), repositoryType);
            return this;
        }

        private static class ChronicleBuilderErrorMessages
        {
            public static string InvalidSagaLogType => "Given type does not derive from ISagaLog interface";
            public static string InvalidSagaDataRepositoryType => "Given type does not derive from ISagaDataRepository interface";
        }
    }
}
