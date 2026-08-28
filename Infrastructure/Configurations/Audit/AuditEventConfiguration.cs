using Domain.Entities.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Audit;

public class AuditEventConfiguration : IEntityTypeConfiguration<AuditEventEntity>
{
    public void Configure(EntityTypeBuilder<AuditEventEntity> builder)
    {
        builder.ToTable("audit_events");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.UserId)
            .HasColumnName("user_id");

        builder.Property(c => c.SessionId)
            .HasColumnName("session_id");

        builder.Property(c => c.EventTypeId)
            .HasColumnName("audit_event_type_id")
            .IsRequired();

        builder.Property(c => c.EntityType)
            .HasColumnName("entity_type")
            .HasMaxLength(80);

        builder.Property(c => c.EntityId)
            .HasColumnName("entity_id");

        builder.Property(c => c.Description)
            .HasColumnName("description")
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(c => c.PreviousData)
            .HasColumnName("previous_data");

        builder.Property(c => c.NewData)
            .HasColumnName("new_data");

        builder.Property(c => c.EventAt)
            .HasColumnName("event_date")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(c => c.Succeeded)
            .HasColumnName("is_success")
            .HasDefaultValueSql("1")
            .IsRequired();

        builder.Property(c => c.ErrorDetail)
            .HasColumnName("error_detail")
            .HasMaxLength(1000);
    }
}
