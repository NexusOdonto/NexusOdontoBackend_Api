using Domain.Entities.Odontogram;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Odontogram;

public class OdontogramToothConfiguration : IEntityTypeConfiguration<OdontogramToothEntity>
{
    public void Configure(EntityTypeBuilder<OdontogramToothEntity> builder)
    {
        builder.ToTable("odontogram_teeth");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.OdontogramId)
            .HasColumnName("odontogram_id")
            .IsRequired();

        builder.Property(c => c.ToothCode)
            .HasColumnName("tooth_code")
            .HasMaxLength(2)
            .IsRequired();

        builder.Property(c => c.ToothStatusId)
            .HasColumnName("tooth_status_id")
            .IsRequired();

        builder.Property(c => c.Note)
            .HasColumnName("notes");

        builder.HasIndex(c => new { c.OdontogramId, c.ToothCode }).IsUnique();
    }
}
