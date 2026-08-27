using DentalSurfaceEntity = Domain.Entities.Catalogs.DentalSurface;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.DentalSurface;

public static class DentalSurfaceSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<DentalSurfaceEntity>().HasData(
            new
            {
                Id = new Guid("1b000000-0000-0000-0000-000000000001"),
                Code = "MESIAL",
                Name = "Mesial",
                Description = "Surface facing the midline of the dental arch.",
                AppliesToAnterior = true,
                AppliesToPosterior = true,
                IsActive = true,
                SortOrder = (int?)1,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("1b000000-0000-0000-0000-000000000002"),
                Code = "DISTAL",
                Name = "Distal",
                Description = "Surface facing away from the midline of the dental arch.",
                AppliesToAnterior = true,
                AppliesToPosterior = true,
                IsActive = true,
                SortOrder = (int?)2,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("1b000000-0000-0000-0000-000000000003"),
                Code = "OCLUSAL",
                Name = "Occlusal",
                Description = "Chewing surface of premolars and molars.",
                AppliesToAnterior = false,
                AppliesToPosterior = true,
                IsActive = true,
                SortOrder = (int?)3,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("1b000000-0000-0000-0000-000000000004"),
                Code = "INCISAL",
                Name = "Incisal",
                Description = "Cutting edge of incisors and canines.",
                AppliesToAnterior = true,
                AppliesToPosterior = false,
                IsActive = true,
                SortOrder = (int?)4,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("1b000000-0000-0000-0000-000000000005"),
                Code = "VESTIBULAR",
                Name = "Buccal",
                Description = "Surface facing the cheek or lips.",
                AppliesToAnterior = true,
                AppliesToPosterior = true,
                IsActive = true,
                SortOrder = (int?)5,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("1b000000-0000-0000-0000-000000000006"),
                Code = "PALATINO",
                Name = "Palatal",
                Description = "Surface of upper teeth facing the palate.",
                AppliesToAnterior = true,
                AppliesToPosterior = true,
                IsActive = true,
                SortOrder = (int?)6,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("1b000000-0000-0000-0000-000000000007"),
                Code = "LINGUAL",
                Name = "Lingual",
                Description = "Surface of lower teeth facing the tongue.",
                AppliesToAnterior = true,
                AppliesToPosterior = true,
                IsActive = true,
                SortOrder = (int?)7,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}
