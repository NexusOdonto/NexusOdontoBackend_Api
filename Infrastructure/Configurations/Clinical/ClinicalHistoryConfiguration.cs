using Domain.Entities.Clinical;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Clinical;

public class ClinicalHistoryConfiguration : IEntityTypeConfiguration<ClinicalHistoryEntity>
{
    public void Configure(EntityTypeBuilder<ClinicalHistoryEntity> builder)
    {
        builder.ToTable("clinical_histories");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.PatientId)
            .HasColumnName("patient_id")
            .IsRequired();

        builder.Property(c => c.OpenedAt)
            .HasColumnName("opening_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(c => c.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(c => c.GeneralNotes)
            .HasColumnName("general_notes");

        builder.HasIndex(c => c.PatientId).IsUnique();
    }
}
