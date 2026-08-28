using Domain.Entities.Clinical;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Clinical;

public class ClinicalAttentionConfiguration : IEntityTypeConfiguration<ClinicalAttentionEntity>
{
    public void Configure(EntityTypeBuilder<ClinicalAttentionEntity> builder)
    {
        builder.ToTable("clinical_attentions");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.AppointmentId)
            .HasColumnName("appointment_id")
            .IsRequired();

        builder.Property(c => c.AttentionAt)
            .HasColumnName("attention_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(c => c.ClinicalExam)
            .HasColumnName("clinical_exam");

        builder.Property(c => c.ProgressNotes)
            .HasColumnName("evolution_notes");

        builder.Property(c => c.TreatmentPlan)
            .HasColumnName("treatment_plan");

        builder.HasIndex(c => c.AppointmentId).IsUnique();
    }
}
