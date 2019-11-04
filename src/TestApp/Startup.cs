using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Chronicle;

namespace TestApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddChronicle();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var coordinator = app.ApplicationServices.GetService<ISagaCoordinator>();

            var context = SagaContext
                .Create()
                .WithSagaId(SagaId.NewSagaId())
                .WithOriginator("Test")
                .WithMetadata("key", "lulz")
                .Build();

            var context2 = SagaContext
                .Create()
                .WithSagaId(SagaId.NewSagaId())
                .WithOriginator("Test")
                .WithMetadata("key", "lulz")
                .Build();

            coordinator.ProcessAsync(new Message1 { Text = "This message will be used one day..." }, context);

            coordinator.ProcessAsync( new Message2 { Text = "But this one will be printed first! (We compensate from the end to beggining of the log)" },
                onCompleted: (m, ctx) =>
                {
                    Console.WriteLine("My work is done");
                    return Task.CompletedTask;
                },
                context: context);

            Console.ReadLine();
        }
    }
}
