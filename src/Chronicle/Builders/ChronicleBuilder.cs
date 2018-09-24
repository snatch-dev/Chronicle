using System;
using Chronicle.Errors;
using Chronicle.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Chronicle.Builders
{
    internal class ChronicleBuilder : IChronicleBuilder
    {
        private readonly IServiceCollection _services;

        public ChronicleBuilder(IServiceCollection services)
            => _services = services;

        public IChronicleBuilder UseInMemoryPersistence()
        {
            _services.AddSingleton(typeof(ISagaDataRepository), typeof(InMemorySagaDataRepository));
            _services.AddSingleton(typeof(ISagaLog), typeof(InMememorySagaLog));
            return this;
        }

        public IChronicleBuilder UseSagaLog(Type sagaLogType)
        {
            Check.Is<ISagaLog>(sagaLogType);
            _services.AddTransient(typeof(ISagaLog), sagaLogType);
            return this;
        }

        public IChronicleBuilder UseSagaDataRepository(Type repositoryType)
        {
            Check.Is<ISagaDataRepository>(repositoryType);
            _services.AddTransient(typeof(ISagaDataRepository), repositoryType);
            return this;
        }

        private static class ChronicleBuilderErrorMessages
        {
            public static string InvalidSagaLogType => "Given type does not derive from ISagaLog interface";
            public static string InvalidSagaDataRepositoryType => "Given type does not derive from ISagaDataRepository interface";
        }
    }
}
