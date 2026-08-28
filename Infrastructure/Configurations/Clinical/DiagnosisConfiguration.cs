using Domain.Entities.Clinical;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Clinical;

public class DiagnosisConfiguration : IEntityTypeConfiguration<DiagnosisEntity>
{
    public void Configure(EntityTypeBuilder<DiagnosisEntity> builder)
    {
        builder.ToTable("diagnoses");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.ClinicalAttentionId)
            .HasColumnName("clinical_attention_id")
            .IsRequired();

        builder.Property(c => c.Code)
            .HasColumnName("code")
            .HasMaxLength(30);

        builder.Property(c => c.Description)
            .HasColumnName("description")
            .IsRequired();

        builder.Property(c => c.Type)
            .HasColumnName("type")
            .HasMaxLength(30)
            .HasDefaultValue("ODONTOLOGICO")
            .IsRequired();
    }
}
