using Domain.Entities.People;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.People;

public class SessionConfiguration : IEntityTypeConfiguration<SessionEntity>
{
    public void Configure(EntityTypeBuilder<SessionEntity> builder)
    {
        builder.ToTable("sessions");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(c => c.RefreshTokenHash)
            .HasColumnName("refresh_token_hash")
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(c => c.TokenFamily)
            .HasColumnName("token_family")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(c => c.ExpiresAt)
            .HasColumnName("expiration_date")
            .IsRequired();

        builder.Property(c => c.LastUsedAt)
            .HasColumnName("last_used_at");

        builder.Property(c => c.RevokedAt)
            .HasColumnName("revoked_at");

        builder.Property(c => c.SessionStatusId)
            .HasColumnName("session_status_id")
            .IsRequired();

        builder.HasIndex(c => c.RefreshTokenHash).IsUnique();
    }
}
