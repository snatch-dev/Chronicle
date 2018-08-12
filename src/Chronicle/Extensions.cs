using Chronicle.Persistance;
using Chronicle.Sagas;
using Microsoft.Extensions.DependencyInjection;

namespace Chronicle
{
    public static class Extensions
    {
        public static void AddChronicle(this IServiceCollection services)
        {
            services.AddTransient(typeof(ISagaCoordinator<,>), typeof(SagaCoordinator<,>));
            services.AddSingleton(typeof(ISagaDataRepository<>), typeof(InMemorySagaDataRepository<>));
            services.AddSingleton(typeof(ISagaLog), typeof(InMememorySagaLog));
        }
    }
}
