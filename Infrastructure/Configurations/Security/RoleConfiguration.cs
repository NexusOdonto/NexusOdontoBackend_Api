using Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Security;

public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.ToTable("roles");

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

        builder.HasIndex(c => c.Code).IsUnique();
        builder.HasIndex(c => c.Name).IsUnique();

        builder.HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity(
                "role_permissions",
                l => l.HasOne(typeof(PermissionEntity)).WithMany().HasForeignKey("permission_id"),
                r => r.HasOne(typeof(RoleEntity)).WithMany().HasForeignKey("role_id"),
                j =>
                {
                    j.HasKey("role_id", "permission_id");
                    j.ToTable("role_permissions");
                    j.Property<Guid>("role_id").HasColumnName("role_id");
                    j.Property<Guid>("permission_id").HasColumnName("permission_id");
                });
    }
}
