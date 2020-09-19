using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Chronicle.Integrations.EFCore.Persistence;

namespace Chronicle.Integrations.EFCore.EntityConfigurations
{
    internal class SagaLogDataEntityTypeConfiguration : IEntityTypeConfiguration<EFCoreSagaLogData>
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
