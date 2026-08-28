using Domain.Entities.Catalogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Catalogs;

public class HubNotificationTypeConfiguration : IEntityTypeConfiguration<HubNotificationTypeEntity>
{
    public void Configure(EntityTypeBuilder<HubNotificationTypeEntity> builder)
    {
        builder.ToTable("hub_notification_types");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Code)
            .HasColumnName("code")
            .HasMaxLength(60)
            .IsRequired();

        builder.Property(c => c.Name)
            .HasColumnName("name")
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(c => c.Description)
            .HasColumnName("description")
            .HasMaxLength(300);

        builder.Property(c => c.Icon)
            .HasColumnName("icon")
            .HasMaxLength(100);

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
