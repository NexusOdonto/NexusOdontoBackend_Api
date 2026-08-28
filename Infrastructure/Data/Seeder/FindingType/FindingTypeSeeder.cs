using FindingTypeEntity = Domain.Entities.Catalogs.FindingTypeEntity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.FindingType;

public static class FindingTypeSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<FindingTypeEntity>().HasData(
            new
            {
                Id = new Guid("1c000000-0000-0000-0000-000000000001"),
                Code = "CARIES",
                Name = "Caries",
                Description = "Carious lesion on a tooth surface.",
                ColorHex = "#F44336",
                VisualSymbol = "CARIES",
                RequiresSurface = true,
                IsActive = true,
                SortOrder = (int?)1,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("1c000000-0000-0000-0000-000000000002"),
                Code = "RESTAURACION",
                Name = "Restoration",
                Description = "Existing filling or restoration on a tooth surface.",
                ColorHex = "#2196F3",
                VisualSymbol = "FILLING",
                RequiresSurface = true,
                IsActive = true,
                SortOrder = (int?)2,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("1c000000-0000-0000-0000-000000000003"),
                Code = "FRACTURA",
                Name = "Fracture",
                Description = "Fracture affecting the tooth structure.",
                ColorHex = "#FF9800",
                VisualSymbol = "FRACTURE",
                RequiresSurface = false,
                IsActive = true,
                SortOrder = (int?)3,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("1c000000-0000-0000-0000-000000000004"),
                Code = "PROTESIS",
                Name = "Prosthesis",
                Description = "Fixed or removable prosthesis on the tooth.",
                ColorHex = "#9C27B0",
                VisualSymbol = "PROSTHESIS",
                RequiresSurface = false,
                IsActive = true,
                SortOrder = (int?)4,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("1c000000-0000-0000-0000-000000000005"),
                Code = "SELLANTE",
                Name = "Sealant",
                Description = "Preventive sealant applied to a tooth surface.",
                ColorHex = "#4CAF50",
                VisualSymbol = "SEALANT",
                RequiresSurface = true,
                IsActive = true,
                SortOrder = (int?)5,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}
