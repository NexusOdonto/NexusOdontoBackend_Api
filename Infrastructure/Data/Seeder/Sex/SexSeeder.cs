using SexEntity = Domain.Entities.Catalogs.SexEntity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.Sex;

public static class SexSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<SexEntity>().HasData(
            new
            {
                Id = new Guid("f0000000-0000-0000-0000-000000000001"),
                Code = "FEMENINO",
                Name = "Female",
                Description = "Female sex.",
                IsActive = true,
                SortOrder = (int?)1,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("f0000000-0000-0000-0000-000000000002"),
                Code = "MASCULINO",
                Name = "Male",
                Description = "Male sex.",
                IsActive = true,
                SortOrder = (int?)2,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("f0000000-0000-0000-0000-000000000003"),
                Code = "OTRO",
                Name = "Other",
                Description = "Other or unspecified sex.",
                IsActive = true,
                SortOrder = (int?)3,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}
