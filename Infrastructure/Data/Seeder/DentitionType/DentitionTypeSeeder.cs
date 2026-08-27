using DentitionTypeEntity = Domain.Entities.Catalogs.DentitionType;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.DentitionType;

public static class DentitionTypeSeeder
{
    //Expuestos porque ToothSeeder los referencia como llave foranea
    public static readonly Guid Permanent = new("19000000-0000-0000-0000-000000000001");
    public static readonly Guid Deciduous = new("19000000-0000-0000-0000-000000000002");

    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<DentitionTypeEntity>().HasData(
            new
            {
                Id = Permanent,
                Code = "PERMANENTE",
                Name = "Permanent",
                Description = "Permanent dentition according to the FDI standard.",
                IsActive = true,
                SortOrder = (int?)1,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Deciduous,
                Code = "TEMPORAL",
                Name = "Deciduous",
                Description = "Primary or deciduous dentition according to the FDI standard.",
                IsActive = true,
                SortOrder = (int?)2,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}
