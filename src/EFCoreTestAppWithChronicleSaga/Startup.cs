using System;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chronicle;
using Chronicle.Integrations.EFCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EFCoreTestApp.Persistence;

namespace EFCoreTestAppWithChronicleSaga
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            /*
                MAKE Sure that both SagaDbContext and ICustomUnitOfWork<SagaDbContext> are registered as Scoped.
                Since Unit of Work is being used, therefore the Chronicle EFCore doesnt save changes to DB,
                infact it gives control back to the application to commit the changes as per requirement.
            */
            services.AddDbContext<SagaDbContext>(builder =>
            {
                var connStr = this.Configuration.GetConnectionString("db");
                builder.UseSqlServer(connStr);
            });
            services.AddScoped<ICustomUnitOfWork<SagaDbContext>, SagaUnitOfWork<SagaDbContext>>();

            services.AddMediatR(new[]{
                typeof(Startup).Assembly
            });

            services.AddMvc().AddNewtonsoftJson();

            static void TestChronicleBuilder(IChronicleBuilder cb)
            {
                cb.UseEFCorePersistence<SagaDbContext>();
            }
            services.AddChronicle(TestChronicleBuilder);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
