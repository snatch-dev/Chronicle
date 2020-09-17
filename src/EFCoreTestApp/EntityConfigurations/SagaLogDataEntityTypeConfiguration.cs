using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCoreTestApp.SagaRepository;

namespace EFCoreTestApp.EntityConfigurations
{
    public class SagaLogDataEntityTypeConfiguration : IEntityTypeConfiguration<EFCoreSagaLogData>
    {
        public void Configure(EntityTypeBuilder<EFCoreSagaLogData> builder)
        {
            builder.ToTable("SagaLog", "dbo");
            builder.HasKey(c => c.logId);
            builder.Property(c => c.logId).ValueGeneratedOnAdd();
            builder.Ignore(c => c.Id);
            builder.Ignore(c => c.Message);
        }
    }
}
