using JobTitleEntity = Domain.Entities.Catalogs.JobTitleEntity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.JobTitle;

public static class JobTitleSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<JobTitleEntity>().HasData(
            new
            {
                Id = new Guid("11000000-0000-0000-0000-000000000001"),
                Code = "ADMINISTRADOR",
                Name = "Administrator",
                Description = "Staff member responsible for clinic administration.",
                IsActive = true,
                SortOrder = (int?)1,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("11000000-0000-0000-0000-000000000002"),
                Code = "RECEPCIONISTA",
                Name = "Receptionist",
                Description = "Staff member responsible for front desk and scheduling.",
                IsActive = true,
                SortOrder = (int?)2,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("11000000-0000-0000-0000-000000000003"),
                Code = "ODONTOLOGO",
                Name = "Dentist",
                Description = "Licensed professional who performs dental procedures.",
                IsActive = true,
                SortOrder = (int?)3,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("11000000-0000-0000-0000-000000000004"),
                Code = "ASISTENTE",
                Name = "Dental Assistant",
                Description = "Staff member who assists during dental procedures.",
                IsActive = true,
                SortOrder = (int?)4,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}
