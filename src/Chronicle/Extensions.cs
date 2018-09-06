using System;
using Chronicle.Builders;
using Chronicle.Sagas;
using Microsoft.Extensions.DependencyInjection;

namespace Chronicle
{
    public static class Extensions
    {
        public static IServiceCollection AddChronicle(this IServiceCollection services, Action<IChronicleBuilder> build = null)
        {
            services.AddTransient(typeof(ISagaCoordinator<,>), typeof(SagaCoordinator<,>));

            var chronicleBuilder = new ChronicleBuilder(services);

            if (build is null)
            {
                chronicleBuilder.UseInMemoryPersistence();
            }
            else
            {
                build(chronicleBuilder);
            }

            return services;
        }
    }
}
