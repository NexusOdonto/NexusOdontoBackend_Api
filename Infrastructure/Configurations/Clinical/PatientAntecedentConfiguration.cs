using Domain.Entities.Clinical;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Clinical;

public class PatientAntecedentConfiguration : IEntityTypeConfiguration<PatientAntecedentEntity>
{
    public void Configure(EntityTypeBuilder<PatientAntecedentEntity> builder)
    {
        builder.ToTable("patient_backgrounds");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.PatientId)
            .HasColumnName("patient_id")
            .IsRequired();

        builder.Property(c => c.AntecedentTypeId)
            .HasColumnName("background_type_id")
            .IsRequired();

        builder.Property(c => c.Description)
            .HasColumnName("description")
            .IsRequired();

        builder.Property(c => c.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(c => c.RegisteredByUserId)
            .HasColumnName("registered_by_user_id")
            .IsRequired();

        builder.Property(c => c.RegisteredAt)
            .HasColumnName("registered_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();
    }
}
