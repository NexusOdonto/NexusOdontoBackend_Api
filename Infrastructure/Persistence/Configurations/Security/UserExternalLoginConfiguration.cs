using Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Security;

public class UserExternalLoginConfiguration : IEntityTypeConfiguration<UserExternalLoginEntity>
{
    public void Configure(EntityTypeBuilder<UserExternalLoginEntity> builder)
    {
        builder.ToTable("user_external_logins");

        builder.HasKey(e => new { e.Provider, e.ProviderKey });

        builder.Property(e => e.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(e => e.Provider)
            .HasColumnName("provider")
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(e => e.ProviderKey)
            .HasColumnName("provider_key")
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(e => e.ProviderEmail)
            .HasColumnName("provider_email")
            .HasMaxLength(150);

        builder.Property(e => e.LinkedAt)
            .HasColumnName("linked_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.HasOne(e => e.UserEntity)
            .WithMany(u => u.ExternalLogins)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
