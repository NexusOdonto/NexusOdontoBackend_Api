using Domain.Entities.Catalogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Catalogs;

public class AppointmentStatusConfiguration : IEntityTypeConfiguration<AppointmentStatusEntity>
{
    public void Configure(EntityTypeBuilder<AppointmentStatusEntity> builder)
    {
        builder.ToTable("appointment_statuses");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Code)
            .HasColumnName("code")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Description)
            .HasColumnName("description")
            .HasMaxLength(300);

        builder.Property(c => c.ColorHex)
            .HasColumnName("color_hex")
            .HasMaxLength(7);

        builder.Property(c => c.SortOrder)
            .HasColumnName("display_order");

        builder.Property(c => c.IsFinal)
            .HasColumnName("is_final")
            .HasDefaultValueSql("0")
            .IsRequired();

        builder.Property(c => c.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValueSql("1")
            .IsRequired();

        builder.HasIndex(c => c.Code).IsUnique();
        builder.HasIndex(c => c.Name).IsUnique();
    }
}
