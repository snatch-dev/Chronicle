using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chronicle;
using Microsoft.EntityFrameworkCore;
using EFCoreTestApp.EntityConfigurations;
using EFCoreTestApp.SagaRepository;

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
           modelBuilder.ApplyConfiguration(new SagaLogDataEntityTypeConfiguration());
           modelBuilder.ApplyConfiguration(new SagaStateEntityTypeConfiguration());
        }

        public DbSet<EFCoreSagaLogData> SagaLog { get; set; }

        public DbSet<EFCoreSagaStateData> SagaState { get; set; }
    }
}
