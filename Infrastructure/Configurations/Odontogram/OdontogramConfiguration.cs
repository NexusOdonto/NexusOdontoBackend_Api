using Domain.Entities.Odontogram;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Odontogram;

public class OdontogramConfiguration : IEntityTypeConfiguration<Domain.Entities.Odontogram.OdontogramEntity>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Odontogram.OdontogramEntity> builder)
    {
        builder.ToTable("odontograms");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.ClinicalHistoryId)
            .HasColumnName("clinical_history_id")
            .IsRequired();

        builder.Property(c => c.ClinicalAttentionId)
            .HasColumnName("clinical_attention_id");

        builder.Property(c => c.ProfessionalId)
            .HasColumnName("professional_id")
            .IsRequired();

        builder.Property(c => c.RecordedAt)
            .HasColumnName("registration_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(c => c.Notes)
            .HasColumnName("notes");
    }
}
