using AppointmentStatusEntity = Domain.Entities.Catalogs.AppointmentStatusEntity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.AppointmentStatus;

public static class AppointmentStatusSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<AppointmentStatusEntity>().HasData(
            new
            {
                Id = new Guid("10000000-0000-0000-0000-000000000001"),
                Code = "AGENDADA",
                Name = "Scheduled",
                Description = "The appointment has been scheduled and is awaiting confirmation.",
                ColorHex = "#3B82F6",
                IsFinal = false,
                IsActive = true,
                SortOrder = (int?)1,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("10000000-0000-0000-0000-000000000002"),
                Code = "CONFIRMADA",
                Name = "Confirmed",
                Description = "The patient has confirmed attendance for the appointment.",
                ColorHex = "#22C55E",
                IsFinal = false,
                IsActive = true,
                SortOrder = (int?)2,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("10000000-0000-0000-0000-000000000003"),
                Code = "EN_SALA",
                Name = "In Waiting Room",
                Description = "The patient has arrived and is waiting to receive care.",
                ColorHex = "#F59E0B",
                IsFinal = false,
                IsActive = true,
                SortOrder = (int?)3,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("10000000-0000-0000-0000-000000000004"),
                Code = "ATENDIDA",
                Name = "Completed",
                Description = "The patient received the scheduled dental care.",
                ColorHex = "#14B8A6",
                IsFinal = true,
                IsActive = true,
                SortOrder = (int?)4,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("10000000-0000-0000-0000-000000000005"),
                Code = "CANCELADA",
                Name = "Cancelled",
                Description = "The appointment was cancelled before dental care was provided.",
                ColorHex = "#EF4444",
                IsFinal = true,
                IsActive = true,
                SortOrder = (int?)5,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("10000000-0000-0000-0000-000000000006"),
                Code = "NO_ASISTIO",
                Name = "No Show",
                Description = "The patient did not attend the scheduled appointment.",
                ColorHex = "#64748B",
                IsFinal = true,
                IsActive = true,
                SortOrder = (int?)6,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}

