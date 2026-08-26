using Domain.Entities.Clinical;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Clinical;

public class ProcedurePerformedConfiguration : IEntityTypeConfiguration<ProcedurePerformed>
{
    public void Configure(EntityTypeBuilder<ProcedurePerformed> builder)
    {
        builder.ToTable("performed_procedures");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.ClinicalAttentionId)
            .HasColumnName("clinical_attention_id")
            .IsRequired();

        builder.Property(c => c.ServiceId)
            .HasColumnName("service_id");

        builder.Property(c => c.Description)
            .HasColumnName("description")
            .IsRequired();

        builder.Property(c => c.PerformedAt)
            .HasColumnName("performed_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(c => c.AmountCharged)
            .HasColumnName("charged_amount")
            .HasPrecision(12, 2);
    }
}
