using SpecialtyEntity = Domain.Entities.Catalogs.SpecialtyEntity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.Specialty;

public static class SpecialtySeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<SpecialtyEntity>().HasData(
            new
            {
                Id = new Guid("18000000-0000-0000-0000-000000000001"),
                Code = "ODONTOLOGIA_GENERAL",
                Name = "General Dentistry",
                Description = "Comprehensive general dental care.",
                IsActive = true,
                SortOrder = (int?)1,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("18000000-0000-0000-0000-000000000002"),
                Code = "ORTODONCIA",
                Name = "Orthodontics",
                Description = "Correction of teeth and jaw alignment.",
                IsActive = true,
                SortOrder = (int?)2,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("18000000-0000-0000-0000-000000000003"),
                Code = "ENDODONCIA",
                Name = "Endodontics",
                Description = "Root canal and dental pulp treatment.",
                IsActive = true,
                SortOrder = (int?)3,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("18000000-0000-0000-0000-000000000004"),
                Code = "PERIODONCIA",
                Name = "Periodontics",
                Description = "Treatment of gums and supporting structures.",
                IsActive = true,
                SortOrder = (int?)4,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("18000000-0000-0000-0000-000000000005"),
                Code = "ODONTOPEDIATRIA",
                Name = "Pediatric Dentistry",
                Description = "Dental care for children and adolescents.",
                IsActive = true,
                SortOrder = (int?)5,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("18000000-0000-0000-0000-000000000006"),
                Code = "CIRUGIA_ORAL",
                Name = "Oral Surgery",
                Description = "Surgical procedures of the mouth and jaw.",
                IsActive = true,
                SortOrder = (int?)6,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}
