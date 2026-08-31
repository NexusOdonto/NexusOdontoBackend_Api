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

        // 1. Administrator & Bot Service get ALL 48 permissions
        for (var m = 1; m <= ModuleCount; m++)
        {
            for (var a = 1; a <= ActionCount; a++)
            {
                AddPermission(RoleSeeder.Administrator, m, a);
                AddPermission(RoleSeeder.BotService, m, a);
            }
        }

        // 2. Dentist (ODONTOLOGO)
        int[] dentistModules = [1, 2, 3, 5, 6, 7, 8, 10]; // Patients, Appointments, Catalogs, Clinical, Odontogram, Notifications, Chatbot, People
        foreach (var m in dentistModules)
        {
            AddPermission(RoleSeeder.Dentist, m, 1); // VIEW
            AddPermission(RoleSeeder.Dentist, m, 2); // CREATE
            AddPermission(RoleSeeder.Dentist, m, 3); // EDIT
            if (m is 2 or 5 or 6) // APPOINTMENTS, CLINICAL, ODONTOGRAM
            {
                AddPermission(RoleSeeder.Dentist, m, 4); // DELETE
            }
        }

        // 3. Receptionist (RECEPCIONISTA)
        int[] receptionistModules = [1, 2, 3, 5, 7, 8, 10]; // Patients, Appointments, Catalogs, Clinical, Notifications, Chatbot, People
        foreach (var m in receptionistModules)
        {
            AddPermission(RoleSeeder.Receptionist, m, 1); // VIEW
            if (m is 1 or 2 or 7 or 8 or 10) // Patients, Appointments, Notifications, Chatbot, People
            {
                AddPermission(RoleSeeder.Receptionist, m, 2); // CREATE
                AddPermission(RoleSeeder.Receptionist, m, 3); // EDIT
            }
            if (m is 1 or 2) // Patients, Appointments
            {
                AddPermission(RoleSeeder.Receptionist, m, 4); // DELETE
            }
        }

        // 4. Assistant (ASISTENTE)
        int[] assistantModules = [1, 2, 3, 5, 6, 7, 10];
        foreach (var m in assistantModules)
        {
            AddPermission(RoleSeeder.Assistant, m, 1); // VIEW
        }

        // 5. Patient (PACIENTE)
        int[] patientModules = [1, 2, 7, 8];
        foreach (var m in patientModules)
        {
            AddPermission(RoleSeeder.Patient, m, 1); // VIEW
            if (m is 1 or 2 or 8) // Patients, Appointments, Chatbot
            {
                AddPermission(RoleSeeder.Patient, m, 2); // CREATE
                AddPermission(RoleSeeder.Patient, m, 3); // EDIT
            }
        }

        modelBuilder.Entity("role_permissions").HasData(data);
    }
}
