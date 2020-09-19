using Microsoft.EntityFrameworkCore;
using Chronicle.Integrations.EFCore.EntityConfigurations;

namespace EFCoreTestApp.Persistence
{
    public class SagaDbContext : DbContext
    {
        public SagaDbContext(DbContextOptions<SagaDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // NOTE: MAKE SURE SAGA MODELS ARE CREATED IN DB
            ConfigureSagaTables.Create(modelBuilder);

            // Configure other models for the Application.
        }
    }
}
