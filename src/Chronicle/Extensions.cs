using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Chronicle.Builders;
using Chronicle.Managers;
using Microsoft.Extensions.DependencyInjection;

namespace Chronicle
{
    public static class Extensions
    {
        public static IServiceCollection AddChronicle(this IServiceCollection services, Action<IChronicleBuilder> build = null)
        {
            services.AddTransient<ISagaCoordinator, SagaCoordinator>();
            services.AddTransient<ISagaSeeker, SagaSeeker>();
            services.AddTransient<ISagaInitializer, SagaInitializer>();
            services.AddTransient<ISagaProcessor, SagaProcessor>();
            services.AddTransient<ISagaPostProcessor, SagaPostProcessor>();

            var chronicleBuilder = new ChronicleBuilder(services);

            if (build is null)
            {
                chronicleBuilder.UseInMemoryPersistence();
            }
            else
            {
                build(chronicleBuilder);
            }

            services.RegisterSagas();

            return services;
        }

        private static void RegisterSagas(this IServiceCollection services)
            => services.Scan(scan =>
            {
                var assembly = Assembly.GetEntryAssembly();

                scan
                    .FromAssemblies(assembly)
                    .AddClasses(classes => classes.AssignableTo(typeof(ISaga)))
                    .As(t => t
                        .GetTypeInfo()
                        .GetInterfaces(includeInherited: false))
                    .WithTransientLifetime();
            });

        private static IEnumerable<Type> GetInterfaces(this Type type, bool includeInherited)
        {
            if (includeInherited || type.BaseType is null)
            {
                return type.GetInterfaces();
            }

            return type.GetInterfaces().Except(type.BaseType.GetInterfaces());
        }
    }
}
