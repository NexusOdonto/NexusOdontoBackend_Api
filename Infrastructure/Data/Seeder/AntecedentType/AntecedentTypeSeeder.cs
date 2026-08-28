using AntecedentTypeEntity = Domain.Entities.Catalogs.AntecedentTypeEntity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.AntecedentType;

public static class AntecedentTypeSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<AntecedentTypeEntity>().HasData(
            new
            {
                Id = new Guid("17000000-0000-0000-0000-000000000001"),
                Code = "MEDICO",
                Name = "Medical",
                Description = "General medical history relevant to dental care.",
                IsActive = true,
                SortOrder = (int?)1,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("17000000-0000-0000-0000-000000000002"),
                Code = "ODONTOLOGICO",
                Name = "Dental",
                Description = "Previous dental treatments and conditions.",
                IsActive = true,
                SortOrder = (int?)2,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("17000000-0000-0000-0000-000000000003"),
                Code = "ALERGIA",
                Name = "Allergy",
                Description = "Known allergies of the patient.",
                IsActive = true,
                SortOrder = (int?)3,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("17000000-0000-0000-0000-000000000004"),
                Code = "MEDICAMENTO",
                Name = "Medication",
                Description = "Medication the patient is currently taking.",
                IsActive = true,
                SortOrder = (int?)4,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}
