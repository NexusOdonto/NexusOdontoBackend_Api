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
                StartTime = new TimeOnly(8, 0),
                LunchStartTime = new TimeOnly(12, 0),
                LunchEndTime = new TimeOnly(13, 0),
                EndTime = new TimeOnly(17, 0),
                IsActive = true
            },
            // 2. Dra. Laura Gómez: Sábados 08:00 - 12:00
            new
            {
                Id = new Guid("1f000000-0000-0000-0000-000000000002"),
                ProfessionalId = dent1Id,
                StartDay = 6, // Sábado
                EndDay = 6,   // Sábado
                StartTime = new TimeOnly(8, 0),
                LunchStartTime = new TimeOnly(12, 0),
                LunchEndTime = new TimeOnly(12, 0),
                EndTime = new TimeOnly(12, 0),
                IsActive = true
            },
            // 3. Dr. Roberto Martínez: Lunes a Viernes 07:00 - 16:00 (Almuerzo 12:00 - 13:00)
            new
            {
                Id = new Guid("1f000000-0000-0000-0000-000000000003"),
                ProfessionalId = dent2Id,
                StartDay = 1, // Lunes
                EndDay = 5,   // Viernes
                StartTime = new TimeOnly(7, 0),
                LunchStartTime = new TimeOnly(12, 0),
                LunchEndTime = new TimeOnly(13, 0),
                EndTime = new TimeOnly(16, 0),
                IsActive = true
            },
            // 4. Dra. Ana Sofía Silva: Lunes a Viernes 09:00 - 18:00 (Almuerzo 13:00 - 14:00)
            new
            {
                Id = new Guid("1f000000-0000-0000-0000-000000000004"),
                ProfessionalId = dent3Id,
                StartDay = 1, // Lunes
                EndDay = 5,   // Viernes
                StartTime = new TimeOnly(9, 0),
                LunchStartTime = new TimeOnly(13, 0),
                LunchEndTime = new TimeOnly(14, 0),
                EndTime = new TimeOnly(18, 0),
                IsActive = true
            });
    }
}
