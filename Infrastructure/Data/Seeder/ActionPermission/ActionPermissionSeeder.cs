using ActionPermissionEntity = Domain.Entities.Catalogs.ActionPermissionEntity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.ActionPermission;

public static class ActionPermissionSeeder
{
    //Expuestos porque PermissionSeeder los referencia como llave foranea
    public static readonly Guid View = new("12000000-0000-0000-0000-000000000001");
    public static readonly Guid Create = new("12000000-0000-0000-0000-000000000002");
    public static readonly Guid Edit = new("12000000-0000-0000-0000-000000000003");
    public static readonly Guid Delete = new("12000000-0000-0000-0000-000000000004");

    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<ActionPermissionEntity>().HasData(
            new
            {
                Id = View,
                Code = "VER",
                Name = "View",
                Description = "Allows reading records of a module.",
                IsActive = true,
                SortOrder = (int?)1,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Create,
                Code = "CREAR",
                Name = "Create",
                Description = "Allows creating records in a module.",
                IsActive = true,
                SortOrder = (int?)2,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Edit,
                Code = "EDITAR",
                Name = "Edit",
                Description = "Allows modifying records of a module.",
                IsActive = true,
                SortOrder = (int?)3,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = Delete,
                Code = "ELIMINAR",
                Name = "Delete",
                Description = "Allows deleting or deactivating records of a module.",
                IsActive = true,
                SortOrder = (int?)4,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}
