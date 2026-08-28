using Infrastructure.Data.Seeder.Role;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.RolePermission;

public static class RolePermissionSeeder
{
    private const int ModuleCount = 8;
    private const int ActionCount = 4;

    public static void Seed(ModelBuilder modelBuilder)
    {
        var data = new List<object>();

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

        modelBuilder.Entity("role_permissions").HasData(data);
    }
}
