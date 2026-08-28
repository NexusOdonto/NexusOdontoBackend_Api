using Infrastructure.Data.Seeder.ActionPermission;
using PermissionEntity = Domain.Entities.Security.PermissionEntity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.Permission;

public static class PermissionSeeder
{
    //Modulos protegidos por permisos. El codigo resultante es MODULO_ACCION,
    //que es el formato usado por las politicas de autorizacion de la capa Api.
    private static readonly string[] Modules =
    [
        "PACIENTES",
        "CITAS",
        "CATALOGOS",
        "USUARIOS",
        "HISTORIAS",
        "ODONTOGRAMA",
        "NOTIFICACIONES",
        "SOPORTE"
    ];

    private static readonly (Guid Id, string Code)[] Actions =
    [
        (ActionPermissionSeeder.View, "VER"),
        (ActionPermissionSeeder.Create, "CREAR"),
        (ActionPermissionSeeder.Edit, "EDITAR"),
        (ActionPermissionSeeder.Delete, "ELIMINAR")
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
                    //Guid determinista: prefijo del catalogo + indice de modulo + indice de accion
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
