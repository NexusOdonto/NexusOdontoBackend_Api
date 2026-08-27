using DocumentTypeEntity = Domain.Entities.Catalogs.DocumentType;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.DocumentType;

public static class DocumentTypeSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<DocumentTypeEntity>().HasData(
            new
            {
                Id = new Guid("e0000000-0000-0000-0000-000000000001"),
                Code = "CC",
                Name = "Citizenship Card",
                Description = "National identity document for Colombian citizens.",
                IsActive = true,
                SortOrder = (int?)1,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("e0000000-0000-0000-0000-000000000002"),
                Code = "CE",
                Name = "Foreigner Card",
                Description = "Identity document for foreign residents.",
                IsActive = true,
                SortOrder = (int?)2,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("e0000000-0000-0000-0000-000000000003"),
                Code = "TI",
                Name = "Identity Card",
                Description = "Identity document for minors.",
                IsActive = true,
                SortOrder = (int?)3,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("e0000000-0000-0000-0000-000000000004"),
                Code = "PAS",
                Name = "Passport",
                Description = "International travel document.",
                IsActive = true,
                SortOrder = (int?)4,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}
