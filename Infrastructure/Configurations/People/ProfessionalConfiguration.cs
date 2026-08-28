using Domain.Entities.Catalogs;
using Domain.Entities.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.People;

public class ProfessionalConfiguration : IEntityTypeConfiguration<ProfessionalEntity>
{
    public void Configure(EntityTypeBuilder<ProfessionalEntity> builder)
    {
        builder.ToTable("professionals");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.EmployeeId)
            .HasColumnName("employee_id")
            .IsRequired();

        builder.Property(c => c.ProfessionalLicense)
            .HasColumnName("professional_license")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValueSql("1")
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasIndex(c => c.EmployeeId).IsUnique();
        builder.HasIndex(c => c.ProfessionalLicense).IsUnique();

        builder.HasOne(p => p.EmployeeEntity)
            .WithOne(e => e.ProfessionalEntity)
            .HasForeignKey<ProfessionalEntity>(p => p.EmployeeId);

        builder.HasMany(p => p.Specialties)
            .WithMany(s => s.Professionals)
            .UsingEntity(
                "professional_specialties",
                l => l.HasOne(typeof(SpecialtyEntity)).WithMany().HasForeignKey("specialty_id"),
                r => r.HasOne(typeof(ProfessionalEntity)).WithMany().HasForeignKey("professional_id"),
                j =>
                {
                    j.HasKey("professional_id", "specialty_id");
                    j.ToTable("professional_specialties");
                    j.Property<Guid>("professional_id").HasColumnName("professional_id");
                    j.Property<Guid>("specialty_id").HasColumnName("specialty_id");
                });
    }
}
