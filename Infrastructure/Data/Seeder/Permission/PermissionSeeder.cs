using Infrastructure.Data.Seeder.ActionPermission;
using PermissionEntity = Domain.Entities.Security.PermissionEntity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.Permission;

public static class PermissionSeeder
{
    // Modules protected by permissions. Resulting code is MODULE_ACTION,
    // the format used by Api authorization policies.
    private static readonly string[] Modules =
    [
        "PATIENTS",
        "APPOINTMENTS",
        "CATALOGS",
        "USERS",
        "CLINICAL",
        "ODONTOGRAM",
        "NOTIFICATIONS",
        "CHATBOT",
        "AUDIT",
        "PEOPLE",
        "ROLES",
        "PERMISSIONS"
    ];

    private static readonly (Guid Id, string Code)[] Actions =
    [
        (ActionPermissionSeeder.View, "VIEW"),
        (ActionPermissionSeeder.Create, "CREATE"),
        (ActionPermissionSeeder.Edit, "EDIT"),
        (ActionPermissionSeeder.Delete, "DELETE")
    ];

    public static void Seed(ModelBuilder modelBuilder)
    {
        var data = new List<object>();

        for (var m = 0; m < Modules.Length; m++)
        {
            for (var a = 0; a < Actions.Length; a++)
            {
                var module = Modules[m];
                var action = Actions[a];

                data.Add(new
                {
                    // Deterministic Guid: catalog prefix + module index + action index
                    Id = new Guid($"14000000-0000-0000-{m + 1:D4}-{a + 1:D12}"),
                    Module = module,
                    ActionPermissionId = action.Id,
                    Description = (string?)$"Allows to {action.Code.ToLowerInvariant()} records of the {module} module."
                });
            }
        }

        modelBuilder.Entity<PermissionEntity>().HasData(data);
    }
}
