using ServiceEntity = Domain.Entities.Catalogs.ServiceEntity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.Service;

public static class ServiceSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<ServiceEntity>().HasData(
            new
            {
                Id = new Guid("30000000-0000-0000-0000-000000000001"),
                Code = "GA",
                Name = "General Assessment",
                Description = "Initial oral examination and general dental assessment.",
                DurationMinutes = 30,
                Price = (decimal?)60000m,
                IsActive = true,
                SortOrder = (int?)1,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("30000000-0000-0000-0000-000000000002"),
                Code = "DP",
                Name = "Dental Prophylaxis",
                Description = "Professional dental cleaning and plaque removal.",
                DurationMinutes = 45,
                Price = (decimal?)120000m,
                IsActive = true,
                SortOrder = (int?)2,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("30000000-0000-0000-0000-000000000003"),
                Code = "CRF",
                Name = "Composite Resin Filling",
                Description = "Restoration of a tooth using composite resin material.",
                DurationMinutes = 60,
                Price = (decimal?)180000m,
                IsActive = true,
                SortOrder = (int?)3,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("30000000-0000-0000-0000-000000000004"),
                Code = "TW",
                Name = "Teeth Whitening",
                Description = "Professional treatment to improve the color of the teeth.",
                DurationMinutes = 90,
                Price = (decimal?)450000m,
                IsActive = true,
                SortOrder = (int?)4,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}

