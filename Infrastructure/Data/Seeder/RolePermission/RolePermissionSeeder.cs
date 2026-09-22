using Infrastructure.Data.Seeder.Role;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.RolePermission;

public static class RolePermissionSeeder
{
    private const int ModuleCount = 12;
    private const int ActionCount = 4;

    private static readonly Guid[] BotMinimalPermissionIds =
    [
        new Guid("14000000-0000-0000-0001-000000000001"), // PATIENTS:VIEW
        new Guid("14000000-0000-0000-0001-000000000002"), // PATIENTS:CREATE
        new Guid("14000000-0000-0000-0002-000000000001"), // APPOINTMENTS:VIEW
        new Guid("14000000-0000-0000-0002-000000000002"), // APPOINTMENTS:CREATE
        new Guid("14000000-0000-0000-0002-000000000003"), // APPOINTMENTS:EDIT
        new Guid("14000000-0000-0000-0002-000000000004"), // APPOINTMENTS:DELETE
        new Guid("14000000-0000-0000-0010-000000000001"), // PEOPLE:VIEW
        new Guid("14000000-0000-0000-0003-000000000001"), // CATALOGS:VIEW
        new Guid("14000000-0000-0000-0008-000000000001"), // CHATBOT:VIEW
        new Guid("14000000-0000-0000-0008-000000000002"), // CHATBOT:CREATE
        new Guid("14000000-0000-0000-0008-000000000003")  // CHATBOT:EDIT
    ];

    public static void Seed(ModelBuilder modelBuilder)
    {
        var data = new List<object>();
        var seededPairs = new HashSet<(Guid RoleId, Guid PermId)>();

        void AddPermission(Guid roleId, int moduleIndex, int actionIndex)
        {
            var permId = new Guid($"14000000-0000-0000-{moduleIndex:D4}-{actionIndex:D12}");
            if (seededPairs.Add((roleId, permId)))
            {
                data.Add(new
                {
                    role_id = roleId,
                    permission_id = permId
                });
            }
        }

        void AddPermissionById(Guid roleId, Guid permissionId)
        {
            if (seededPairs.Add((roleId, permissionId)))
            {
                data.Add(new
                {
                    role_id = roleId,
                    permission_id = permissionId
                });
            }
        }

        // Administrator gets ALL 48 permissions
        for (var m = 1; m <= ModuleCount; m++)
        {
            for (var a = 1; a <= ActionCount; a++)
            {
                AddPermission(RoleSeeder.Administrator, m, a);
            }
        }

        // Bot Service gets minimum required permissions only
        foreach (var permissionId in BotMinimalPermissionIds)
        {
            AddPermissionById(RoleSeeder.BotService, permissionId);
        }

        // Dentist (ODONTOLOGO)
        int[] dentistModules = [1, 2, 3, 5, 6, 7, 8, 10];
        foreach (var m in dentistModules)
        {
            AddPermission(RoleSeeder.Dentist, m, 1);
            AddPermission(RoleSeeder.Dentist, m, 2);
            AddPermission(RoleSeeder.Dentist, m, 3);
            if (m is 2 or 5 or 6)
            {
                AddPermission(RoleSeeder.Dentist, m, 4);
            }
        }

        // Receptionist (RECEPCIONISTA)
        int[] receptionistModules = [1, 2, 3, 5, 7, 8, 10];
        foreach (var m in receptionistModules)
        {
            AddPermission(RoleSeeder.Receptionist, m, 1);
            if (m is 1 or 2 or 7 or 8 or 10)
            {
                AddPermission(RoleSeeder.Receptionist, m, 2);
                AddPermission(RoleSeeder.Receptionist, m, 3);
            }
            else if (m is 5)
            {
                // Recepción registra antecedentes (alergias) al dar de alta al
                // paciente, así que necesita CLINICAL:CREATE. Deliberadamente no
                // recibe EDIT ni DELETE: corregir historia clínica ya registrada
                // sigue siendo del odontólogo.
                AddPermission(RoleSeeder.Receptionist, m, 2);
            }
            if (m is 1 or 2)
            {
                AddPermission(RoleSeeder.Receptionist, m, 4);
            }
        }

        // Assistant (ASISTENTE)
        int[] assistantModules = [1, 2, 3, 5, 6, 7, 10];
        foreach (var m in assistantModules)
        {
            AddPermission(RoleSeeder.Assistant, m, 1);
        }

        // Patient (PACIENTE)
        int[] patientModules = [1, 2, 3, 7, 8];
        foreach (var m in patientModules)
        {
            AddPermission(RoleSeeder.Patient, m, 1);
            if (m is 1 or 2 or 8)
            {
                AddPermission(RoleSeeder.Patient, m, 2);
                AddPermission(RoleSeeder.Patient, m, 3);
            }
        }

        modelBuilder.Entity("role_permissions").HasData(data);
    }
}
