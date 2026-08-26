using Domain.Entities.Odontogram;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Odontogram;

public class ToothConfiguration : IEntityTypeConfiguration<Tooth>
{
    public void Configure(EntityTypeBuilder<Tooth> builder)
    {
        builder.ToTable("teeth");

        builder.HasKey(c => c.Code);

        builder.Property(c => c.Code)
            .HasColumnName("code")
            .HasMaxLength(2)
            .IsRequired();

        builder.Property(c => c.Name)
            .HasColumnName("name")
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(c => c.DentitionTypeId)
            .HasColumnName("dentition_type_id")
            .IsRequired();

        builder.Property(c => c.Quadrant)
            .HasColumnName("quadrant")
            .IsRequired();

        builder.Property(c => c.Position)
            .HasColumnName("position")
            .IsRequired();
    }
}
