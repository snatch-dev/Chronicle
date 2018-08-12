using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Chronicle;

namespace TestApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddChronicle();
            services.AddTransient<SampleSaga, SampleSaga>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var coordinator = app.ApplicationServices.GetService<ISagaCoordinator<SampleSaga, SagaData>>();

            var id = Guid.NewGuid();

            coordinator.DispatchAsync(id, new Message1
            {
                Text = "This message will be used one day..."
            });

            coordinator.DispatchAsync(id, new Message2
            {
                Text = "But this one will be printed first! (We compensate from the end to beggining of the log)"
            });

            Console.ReadLine();
        }
    }
}
