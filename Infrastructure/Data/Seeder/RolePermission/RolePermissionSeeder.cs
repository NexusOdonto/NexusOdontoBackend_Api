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

        // Administrator & Bot Service get all permissions (12 modules * 4 actions = 48 permissions)
        for (var m = 0; m < ModuleCount; m++)
        {
            for (var a = 0; a < ActionCount; a++)
            {
                var permId = new Guid($"14000000-0000-0000-{m + 1:D4}-{a + 1:D12}");

                data.Add(new
                {
                    role_id = RoleSeeder.Administrator,
                    permission_id = permId
                });

                data.Add(new
                {
                    role_id = RoleSeeder.BotService,
                    permission_id = permId
                });
            }
        }

        modelBuilder.Entity("role_permissions").HasData(data);
    }
}
