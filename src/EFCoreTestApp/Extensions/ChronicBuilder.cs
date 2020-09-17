using System;
using Chronicle;
using Microsoft.Extensions.DependencyInjection;
using EFCoreTestApp.SagaRepository;

namespace EFCoreTestApp.Extensions
{
    public static class ChronicBuilder
    {
        public static IServiceCollection UserEfCoreForSaga(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(typeof(ISagaStateRepository), typeof(EFCoreSagaState));
            serviceCollection.AddTransient(typeof(ISagaLog), typeof(EFCoreSagaLog));
            return serviceCollection;
        }
    }
}
