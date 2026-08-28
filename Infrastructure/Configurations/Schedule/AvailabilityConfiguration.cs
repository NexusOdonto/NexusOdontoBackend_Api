using Domain.Entities.Schedule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Schedule;

public class AvailabilityConfiguration : IEntityTypeConfiguration<AvailabilityEntity>
{
    public void Configure(EntityTypeBuilder<AvailabilityEntity> builder)
    {
        builder.ToTable("availabilities");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.ProfessionalId)
            .HasColumnName("professional_id")
            .IsRequired();

        builder.Property(c => c.StartDay)
            .HasColumnName("start_day")
            .IsRequired();

        builder.Property(c => c.EndDay)
            .HasColumnName("end_day")
            .IsRequired();

        builder.Property(c => c.StartTime)
            .HasColumnName("start_time")
            .HasMaxLength(5)
            .IsRequired();

        builder.Property(c => c.LunchStartTime)
            .HasColumnName("lunch_start_time")
            .HasMaxLength(5)
            .IsRequired();

        builder.Property(c => c.LunchEndTime)
            .HasColumnName("lunch_end_time")
            .HasMaxLength(5)
            .IsRequired();

        builder.Property(c => c.EndTime)
            .HasColumnName("end_time")
            .HasMaxLength(5)
            .IsRequired();

        builder.Property(c => c.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValueSql("1")
            .IsRequired();
    }
}
