using ToothStatusEntity = Domain.Entities.Catalogs.ToothStatusEntity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.ToothStatus;

public static class ToothStatusSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<ToothStatusEntity>().HasData(
            new
            {
                Id = new Guid("1a000000-0000-0000-0000-000000000001"),
                Code = "SANO",
                Name = "Healthy",
                Description = "Tooth without any detected condition.",
                ColorHex = "#FFFFFF",
                VisualCode = "HEALTHY",
                IsActive = true,
                SortOrder = (int?)1,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("1a000000-0000-0000-0000-000000000002"),
                Code = "AUSENTE",
                Name = "Missing",
                Description = "Tooth is not present and was never erupted or is congenitally absent.",
                ColorHex = "#9E9E9E",
                VisualCode = "MISSING",
                IsActive = true,
                SortOrder = (int?)2,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("1a000000-0000-0000-0000-000000000003"),
                Code = "EXTRAIDO",
                Name = "Extracted",
                Description = "Tooth was removed through extraction.",
                ColorHex = "#616161",
                VisualCode = "EXTRACTED",
                IsActive = true,
                SortOrder = (int?)3,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("1a000000-0000-0000-0000-000000000004"),
                Code = "IMPLANTE",
                Name = "Implant",
                Description = "Tooth was replaced by a dental implant.",
                ColorHex = "#3F51B5",
                VisualCode = "IMPLANT",
                IsActive = true,
                SortOrder = (int?)4,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("1a000000-0000-0000-0000-000000000005"),
                Code = "CORONA",
                Name = "Crown",
                Description = "Tooth is restored with a full crown.",
                ColorHex = "#FFC107",
                VisualCode = "CROWN",
                IsActive = true,
                SortOrder = (int?)5,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}
