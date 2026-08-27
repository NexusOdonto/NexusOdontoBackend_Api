using AppointmentOriginEntity = Domain.Entities.Catalogs.AppointmentOrigin;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.AppointmentOrigin;

public static class AppointmentOriginSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<AppointmentOriginEntity>().HasData(
            new
            {
                Id = new Guid("20000000-0000-0000-0000-000000000001"),
                Code = "MANUAL",
                Name = "Manual",
                Description = "The appointment was created manually by authorized personnel.",
                IsActive = true,
                SortOrder = (int?)1,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("20000000-0000-0000-0000-000000000002"),
                Code = "AGENTE_BOT",
                Name = "Bot Agent",
                Description = "The appointment was created through the automated assistant.",
                IsActive = true,
                SortOrder = (int?)2,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("20000000-0000-0000-0000-000000000003"),
                Code = "WEB",
                Name = "Web",
                Description = "The appointment was requested through the web application.",
                IsActive = true,
                SortOrder = (int?)3,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("20000000-0000-0000-0000-000000000004"),
                Code = "TELEFONO",
                Name = "Telephone",
                Description = "The appointment was requested during a telephone call.",
                IsActive = true,
                SortOrder = (int?)4,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}

