using WhatsAppNotificationTypeEntity = Domain.Entities.Catalogs.WhatsAppNotificationTypeEntity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.WhatsAppNotificationType;

public static class WhatsAppNotificationTypeSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<WhatsAppNotificationTypeEntity>().HasData(
            new
            {
                Id = new Guid("70000000-0000-0000-0000-000000000001"),
                Code = "RECORDATORIO_CITA",
                Name = "Appointment Reminder",
                Description = "Reminds a patient about an upcoming dental appointment.",
                DefaultTemplate = "appointment_reminder",
                IsActive = true,
                SortOrder = (int?)1,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("70000000-0000-0000-0000-000000000002"),
                Code = "CONFIRMACION_CITA",
                Name = "Appointment Confirmation",
                Description = "Confirms the details of a scheduled dental appointment.",
                DefaultTemplate = "appointment_confirmation",
                IsActive = true,
                SortOrder = (int?)2,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("70000000-0000-0000-0000-000000000003"),
                Code = "TRANSFERENCIA_A_ASESOR",
                Name = "Advisor Transfer",
                Description = "Informs the user that the conversation is being transferred to a human advisor.",
                DefaultTemplate = "advisor_transfer",
                IsActive = true,
                SortOrder = (int?)3,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("70000000-0000-0000-0000-000000000004"),
                Code = "BIENVENIDA",
                Name = "Welcome",
                Description = "Welcomes a user to the dental care communication channel.",
                DefaultTemplate = "welcome",
                IsActive = true,
                SortOrder = (int?)4,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}

