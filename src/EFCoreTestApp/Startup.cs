using System;
using MediatR;
using Chronicle;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using EFCoreTestApp.SagaRepository;
using EFCoreTestApp.Persistence;
// Inorder to use Chronicles internal EFCore Implementation
// using Chronicle.Integrations.EFCore;

namespace EFCoreTestApp
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        public IConfiguration Configuration { get; }
        private static string _connectionString;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            Configuration = configuration;
            _connectionString = configuration.GetConnectionString("db");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddControllers();
            /*
                NOTE: Remove AddDbContext<SagaDbContext> if the Chronicles Internal EFCore/SQL Server implementation is being used.
            */
            services.AddDbContext<SagaDbContext>(builder =>
            {
                var connStr = this.Configuration.GetConnectionString("db");
                builder.UseSqlServer(connStr);
            });

            /*
               NOTE: Enable only if Chroniel Internal Implementation needs to be used.
           */
            /*static void TestChronicleBuilder(IChronicleBuilder cb)
            {
                cb.UseEFCorePersistence(_connectionString);
            }*/

            /*
               NOTE: Remove this if the Chronicles Internal EFCore/SQL Server implementation is being used.
           */
            static void TestChronicleBuilder(IChronicleBuilder cb)
            {
                cb.UseSagaLog<EFCoreSagaLog>();
                cb.UseSagaStateRepository<EFCoreSagaState>();
            }

            services.AddChronicle(TestChronicleBuilder);

            /*
               NOTE: Remove this if the Chronicles Internal EFCore/SQL Server implementation is being used.
            */
            services.AddScoped<ISagaLogRepository, SagaLogRepository>();
            services.AddScoped<ISagaStateDBRepository, SagaStateRepository>();
            services.AddScoped<ISagaUnitOfWork, SagaUnitOfWork>();

            services.AddMediatR(new[]{
                typeof(Startup).Assembly
            });

            services.AddMvc().AddNewtonsoftJson();

        }

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
