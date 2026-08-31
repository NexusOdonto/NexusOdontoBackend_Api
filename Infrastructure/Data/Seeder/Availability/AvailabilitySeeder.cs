using Domain.Entities.Schedule;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.Availability;

public static class AvailabilitySeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var dent1Id = new Guid("1d000000-0000-0000-0000-000000000001"); // Dra. Laura Gómez
        var dent2Id = new Guid("1d000000-0000-0000-0000-000000000002"); // Dr. Roberto Martínez
        var dent3Id = new Guid("1d000000-0000-0000-0000-000000000003"); // Dra. Ana Sofía Silva

        modelBuilder.Entity<AvailabilityEntity>().HasData(
            // 1. Dra. Laura Gómez: Lunes a Viernes 08:00 - 17:00 (Almuerzo 12:00 - 13:00)
            new
            {
                Id = new Guid("1f000000-0000-0000-0000-000000000001"),
                ProfessionalId = dent1Id,
                StartDay = 1, // Lunes
                EndDay = 5,   // Viernes
                StartTime = "08:00",
                LunchStartTime = "12:00",
                LunchEndTime = "13:00",
                EndTime = "17:00",
                IsActive = true
            },
            // 2. Dra. Laura Gómez: Sábados 08:00 - 12:00
            new
            {
                Id = new Guid("1f000000-0000-0000-0000-000000000002"),
                ProfessionalId = dent1Id,
                StartDay = 6, // Sábado
                EndDay = 6,   // Sábado
                StartTime = "08:00",
                LunchStartTime = "12:00",
                LunchEndTime = "12:00",
                EndTime = "12:00",
                IsActive = true
            },
            // 3. Dr. Roberto Martínez: Lunes a Viernes 07:00 - 16:00 (Almuerzo 12:00 - 13:00)
            new
            {
                Id = new Guid("1f000000-0000-0000-0000-000000000003"),
                ProfessionalId = dent2Id,
                StartDay = 1, // Lunes
                EndDay = 5,   // Viernes
                StartTime = "07:00",
                LunchStartTime = "12:00",
                LunchEndTime = "13:00",
                EndTime = "16:00",
                IsActive = true
            },
            // 4. Dra. Ana Sofía Silva: Lunes a Viernes 09:00 - 18:00 (Almuerzo 13:00 - 14:00)
            new
            {
                Id = new Guid("1f000000-0000-0000-0000-000000000004"),
                ProfessionalId = dent3Id,
                StartDay = 1, // Lunes
                EndDay = 5,   // Viernes
                StartTime = "09:00",
                LunchStartTime = "13:00",
                LunchEndTime = "14:00",
                EndTime = "18:00",
                IsActive = true
            });
    }
}
