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
using EFCoreTestApp.Extensions;

namespace EFCoreTestApp
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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContextPool<SagaDbContext>(builder =>
            {
                var connStr = this.Configuration.GetConnectionString("db");
                builder.UseSqlServer(connStr);
            });
            
            services.AddScoped<ISagaLogRepository, SagaLogRepository>();
            services.AddScoped<ISagaStateDBRepository, SagaStateRepository>();
            services.AddScoped<ISagaUnitOfWork, SagaUnitOfWork>();

            services.UserEfCoreForSaga();

            services.AddMediatR(new[]{
                typeof(Startup).Assembly
            });

            services.AddMvc().AddNewtonsoftJson();

            static void TestChronicleBuilder(IChronicleBuilder cb)
            {
                cb.UseSagaLog<EFCoreSagaLog>();
                cb.UseSagaStateRepository<EFCoreSagaState>();
            }
            services.AddChronicle(TestChronicleBuilder);

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
