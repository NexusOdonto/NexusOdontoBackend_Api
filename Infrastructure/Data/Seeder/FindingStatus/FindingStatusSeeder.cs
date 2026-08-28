using FindingStatusEntity = Domain.Entities.Catalogs.FindingStatusEntity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.FindingStatus;

public static class FindingStatusSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<FindingStatusEntity>().HasData(
            new
            {
                Id = new Guid("1d000000-0000-0000-0000-000000000001"),
                Code = "ACTIVO",
                Name = "Active",
                Description = "Finding is present and pending treatment.",
                ColorHex = "#F44336",
                IsActive = true,
                SortOrder = (int?)1,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("1d000000-0000-0000-0000-000000000002"),
                Code = "TRATADO",
                Name = "Treated",
                Description = "Finding was treated and resolved.",
                ColorHex = "#4CAF50",
                IsActive = true,
                SortOrder = (int?)2,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("1d000000-0000-0000-0000-000000000003"),
                Code = "INACTIVO",
                Name = "Inactive",
                Description = "Finding is no longer relevant for follow up.",
                ColorHex = "#9E9E9E",
                IsActive = true,
                SortOrder = (int?)3,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}
