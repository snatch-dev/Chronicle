using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCoreTestApp.SagaRepository;

namespace EFCoreTestApp.EntityConfigurations
{
    public class SagaStateEntityTypeConfiguration : IEntityTypeConfiguration<EFCoreSagaStateData>
    {
        public void Configure(EntityTypeBuilder<EFCoreSagaStateData> builder)
        {
            builder.ToTable("SagaState", "dbo");
            builder.HasKey(r => r.SagaId);
            builder.Ignore(c => c.Id);
            builder.Ignore(c => c.Data);
        }
    }
}
