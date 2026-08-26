using Domain.Entities.Schedule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Schedule;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("appointments");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.PatientId)
            .HasColumnName("patient_id")
            .IsRequired();

        builder.Property(c => c.ProfessionalId)
            .HasColumnName("professional_id")
            .IsRequired();

        builder.Property(c => c.ServiceId)
            .HasColumnName("service_id")
            .IsRequired();

        builder.Property(c => c.StartsAt)
            .HasColumnName("start_date_time")
            .IsRequired();

        builder.Property(c => c.EndsAt)
            .HasColumnName("end_date_time")
            .IsRequired();

        builder.Property(c => c.AppointmentStatusId)
            .HasColumnName("appointment_status_id")
            .IsRequired();

        builder.Property(c => c.AppointmentOriginId)
            .HasColumnName("appointment_origin_id")
            .IsRequired();

        builder.Property(c => c.ReasonForVisit)
            .HasColumnName("chief_complaint")
            .HasMaxLength(1000);

        builder.Property(c => c.Notes)
            .HasColumnName("notes")
            .HasMaxLength(2000);

        builder.Property(c => c.CreatedByUserId)
            .HasColumnName("created_by_user_id");

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(c => c.CancelledAt)
            .HasColumnName("canceled_at");

        builder.Property(c => c.CancellationReason)
            .HasColumnName("cancellation_reason")
            .HasMaxLength(1000);

        builder.HasIndex(c => new { c.ProfessionalId, c.StartsAt });
        builder.HasIndex(c => new { c.PatientId, c.StartsAt });
    }
}
