using Microsoft.EntityFrameworkCore;
using Chronicle.Integrations.EFCore.EntityConfigurations;
using Chronicle.Integrations.EFCore.Persistence;


namespace Chronicle.Integrations.EFCore.Repositories
{
    internal class SagaDbContext : DbContext
    {
        public SagaDbContext(DbContextOptions<SagaDbContext> options)
            : base(options)
        {
            // This can be removed and tables can be created manually as part of an Initialization Script
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SagaLogDataEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SagaStateEntityTypeConfiguration());
        }

        public DbSet<EFCoreSagaLogData> SagaLog { get; set; }

        public DbSet<EFCoreSagaStateData> SagaState { get; set; }
    }
}
