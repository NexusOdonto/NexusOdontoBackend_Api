using Infrastructure.Data.Seeder.Role;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.RolePermission;

public static class RolePermissionSeeder
{
    private const int ModuleCount = 12;
    private const int ActionCount = 4;

    public static void Seed(ModelBuilder modelBuilder)
    {
        var data = new List<object>();

        // 1. Administrator gets all permissions
        for (var m = 0; m < ModuleCount; m++)
        {
            for (var a = 0; a < ActionCount; a++)
            {
                data.Add(new
                {
                    role_id = RoleSeeder.Administrator,
                    permission_id = new Guid($"14000000-0000-0000-{m + 1:D4}-{a + 1:D12}")
                });
            }
        }

        // 2. Bot Service gets minimum required permissions (Principle of Least Privilege)
        var botPermissions = new[]
        {
            new Guid("14000000-0000-0000-0001-000000000001"), // PATIENTS:VIEW
            new Guid("14000000-0000-0000-0001-000000000002"), // PATIENTS:CREATE
            new Guid("14000000-0000-0000-0002-000000000001"), // APPOINTMENTS:VIEW
            new Guid("14000000-0000-0000-0002-000000000002"), // APPOINTMENTS:CREATE
            new Guid("14000000-0000-0000-0002-000000000003"), // APPOINTMENTS:EDIT
            new Guid("14000000-0000-0000-0003-000000000001"), // CATALOGS:VIEW
            new Guid("14000000-0000-0000-0004-000000000002"), // USERS:CREATE
            new Guid("14000000-0000-0000-0007-000000000001"), // NOTIFICATIONS:VIEW
            new Guid("14000000-0000-0000-0007-000000000002"), // NOTIFICATIONS:CREATE
            new Guid("14000000-0000-0000-0008-000000000001"), // CHATBOT:VIEW
            new Guid("14000000-0000-0000-0008-000000000002"), // CHATBOT:CREATE
            new Guid("14000000-0000-0000-0008-000000000003"), // CHATBOT:EDIT
            new Guid("14000000-0000-0000-0011-000000000002")  // PEOPLE:CREATE
        };

        foreach (var permId in botPermissions)
        {
            data.Add(new
            {
                role_id = RoleSeeder.BotService,
                permission_id = permId
            });
        }

        modelBuilder.Entity("role_permissions").HasData(data);
    }
}
