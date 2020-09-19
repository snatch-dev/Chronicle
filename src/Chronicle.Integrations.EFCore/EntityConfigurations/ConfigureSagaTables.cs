using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Chronicle.Integrations.EFCore.EntityConfigurations
{
    public static class ConfigureSagaTables
    {
        public static void Create(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SagaLogDataEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SagaStateEntityTypeConfiguration());
        }
    }
}
