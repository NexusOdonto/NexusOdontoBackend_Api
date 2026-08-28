using Domain.Entities.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.People;

public class PatientConfiguration : IEntityTypeConfiguration<PatientEntity>
{
    public void Configure(EntityTypeBuilder<PatientEntity> builder)
    {
        builder.ToTable("patients");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.PersonId)
            .HasColumnName("person_id")
            .IsRequired();

        builder.Property(c => c.EmergencyContact)
            .HasColumnName("emergency_contact")
            .HasMaxLength(150);

        builder.Property(c => c.EmergencyPhone)
            .HasColumnName("emergency_phone")
            .HasMaxLength(30);

        builder.Property(c => c.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .HasColumnName("registration_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasIndex(c => c.PersonId).IsUnique();

        builder.HasOne(c => c.PersonEntity)
            .WithOne(p => p.PatientEntity)
            .HasForeignKey<PatientEntity>(c => c.PersonId);
    }
}
