using Domain.Entities.Odontogram;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Odontogram;

public class DentalFindingConfiguration : IEntityTypeConfiguration<DentalFindingEntity>
{
    public void Configure(EntityTypeBuilder<DentalFindingEntity> builder)
    {
        builder.ToTable("dental_findings");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.OdontogramToothId)
            .HasColumnName("odontogram_tooth_id")
            .IsRequired();

        builder.Property(c => c.DentalSurfaceId)
            .HasColumnName("tooth_surface_id");

        builder.Property(c => c.FindingTypeId)
            .HasColumnName("finding_type_id")
            .IsRequired();

        builder.Property(c => c.FindingStatusId)
            .HasColumnName("finding_status_id")
            .IsRequired();

        builder.Property(c => c.Note)
            .HasColumnName("notes");

        builder.Property(c => c.RegisteredAt)
            .HasColumnName("registered_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();
    }
}
