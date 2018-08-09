using Autofac;
using System;
using System.Threading.Tasks;

namespace Saga
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<SampleSaga>().AsSelf();
            containerBuilder.RegisterType<SagaCoordinator>().As<ISagaCoordinator>();
            containerBuilder.RegisterGeneric(typeof(InMemorySagaDataRepository<>)).As(typeof(ISagaDataRepository<>)).SingleInstance();

            var container = containerBuilder.Build();

            var sc = container.Resolve<ISagaCoordinator>();

            var id = Guid.NewGuid();

            await sc.DispatchAsync<SampleSaga, Message1>(new Message1(), id);

            var state1 = await sc.GetStatusAsync<SampleSaga>(id);

            Console.WriteLine(state1);

            await sc.DispatchAsync<SampleSaga, Message2>(new Message2(), id);

            var state2 = await sc.GetStatusAsync<SampleSaga>(id);

            Console.WriteLine(state2);

            Console.ReadKey();
        }
    }
}
