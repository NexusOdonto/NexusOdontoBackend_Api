using Domain.Entities.Catalogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Catalogs;

public class ServiceConfiguration : IEntityTypeConfiguration<ServiceEntity>
{
    public void Configure(EntityTypeBuilder<ServiceEntity> builder)
    {
        builder.ToTable("services");

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
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(c => c.Description)
            .HasColumnName("description");

        builder.Property(c => c.DurationMinutes)
            .HasColumnName("duration_minutes")
            .IsRequired();

        builder.Property(c => c.Price)
            .HasColumnName("price")
            .HasPrecision(12, 2);

        builder.Property(c => c.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValueSql("1")
            .IsRequired();

        builder.Property(c => c.SortOrder)
            .HasColumnName("display_order");

        builder.HasIndex(c => c.Code).IsUnique();
        builder.HasIndex(c => c.Name).IsUnique();
    }
}
