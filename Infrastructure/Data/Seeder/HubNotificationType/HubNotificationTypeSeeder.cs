using HubNotificationTypeEntity = Domain.Entities.Catalogs.HubNotificationTypeEntity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.HubNotificationType;

public static class HubNotificationTypeSeeder
{
    public static readonly Guid ScheduleUpdatedId = new("40000000-0000-0000-0000-000000000005");

    /// <summary>
    /// Types added after the initial migrations. Inserted idempotently at startup by
    /// DatabaseInitializer.EnsureRuntimeCatalogRowsAsync instead of HasData, so production gets them without a migration.
    /// </summary>
    public static readonly (Guid Id, string Code, string Name, string Description, string Icon, int SortOrder)[] RuntimeRows =
    [
        (ScheduleUpdatedId, "HORARIO_ACTUALIZADO", "Schedule Updated",
            "A professional's weekly schedule was changed; it applies from the next day.",
            "calendar-clock", 5)
    ];

    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<HubNotificationTypeEntity>().HasData(
            new
            {
                Id = new Guid("40000000-0000-0000-0000-000000000001"),
                Code = "TICKET_CREADO",
                Name = "Ticket Created",
                Description = "A new support ticket has been created.",
                Icon = "ticket",
                IsActive = true,
                SortOrder = (int?)1,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("40000000-0000-0000-0000-000000000002"),
                Code = "CITA_CREADA",
                Name = "Appointment Created",
                Description = "A new dental appointment has been created.",
                Icon = "calendar-plus",
                IsActive = true,
                SortOrder = (int?)2,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("40000000-0000-0000-0000-000000000003"),
                Code = "CITA_CANCELADA",
                Name = "Appointment Cancelled",
                Description = "A scheduled dental appointment has been cancelled.",
                Icon = "calendar-x",
                IsActive = true,
                SortOrder = (int?)3,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("40000000-0000-0000-0000-000000000004"),
                Code = "PACIENTE_REGISTRADO",
                Name = "Patient Registered",
                Description = "A new patient has been registered in the system.",
                Icon = "user-plus",
                IsActive = true,
                SortOrder = (int?)4,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}

