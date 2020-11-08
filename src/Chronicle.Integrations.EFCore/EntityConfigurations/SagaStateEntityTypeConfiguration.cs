using Chronicle.Integrations.EFCore.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chronicle.Integrations.EFCore.EntityConfigurations
{
    internal class SagaStateEntityTypeConfiguration : IEntityTypeConfiguration<EFCoreSagaStateData>
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
