using Domain.Entities.Catalogs;
using Domain.Entities.People;
using Domain.Entities.Security;
using Domain.Entities.Schedule;
using Infrastructure.Data.Seeder.AdminUser;
using Infrastructure.Data.Seeder.Employee;
using Infrastructure.Data.Seeder.Professional;
using Infrastructure.Data.Seeder.Patient;
using Infrastructure.Data.Seeder.Role;
using Infrastructure.Data.Seeder.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Context;

public static class DatabaseInitializer
{
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

    public static async Task SeedBotUserAsync(IServiceProvider serviceProvider)
    {
        await SeedAllDataAsync(serviceProvider);
    }

    public static async Task SeedAllDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetService<ILogger<AppDbContext>>();

        try
        {
            var allPermissions = await context.Set<PermissionEntity>().ToListAsync();

            var rolesToEnsure = new (Guid Id, string Code, string Name)[]
            {
                (RoleSeeder.Administrator, "ADMINISTRADOR", "Administrator"),
                (RoleSeeder.Dentist, "ODONTOLOGO", "Dentist"),
                (RoleSeeder.Receptionist, "RECEPCIONISTA", "Receptionist"),
                (RoleSeeder.Assistant, "ASISTENTE", "Dental Assistant"),
                (RoleSeeder.Patient, "PACIENTE", "Patient"),
                (RoleSeeder.BotService, "BOT_SERVICE", "Bot Service")
            };

            foreach (var (rId, rCode, rName) in rolesToEnsure)
            {
                var role = await context.Set<RoleEntity>()
                    .Include(r => r.Permissions)
                    .FirstOrDefaultAsync(r => r.Id == rId || r.Code == rCode);

                if (role is null)
                {
                    role = new RoleEntity(rCode, rName, $"Role for {rName}");
                    SetEntityId(role, rId);
                    await context.Set<RoleEntity>().AddAsync(role);
                    await context.SaveChangesAsync();
                }

                if (rCode == "ADMINISTRADOR")
                {
                    var existingPermIds = role.Permissions.Select(p => p.Id).ToHashSet();
                    var missing = allPermissions.Where(p => !existingPermIds.Contains(p.Id)).ToList();
                    if (missing.Count > 0)
                    {
                        foreach (var perm in missing)
                        {
                            role.Permissions.Add(perm);
                        }

                        await context.SaveChangesAsync();
                    }
                }
                else if (rCode == "BOT_SERVICE")
                {
                    var minimalPermissions = allPermissions
                        .Where(p => BotMinimalPermissionIds.Contains(p.Id))
                        .ToList();

                    var toRemove = role.Permissions
                        .Where(p => !BotMinimalPermissionIds.Contains(p.Id))
                        .ToList();

                    foreach (var perm in toRemove)
                    {
                        role.Permissions.Remove(perm);
                    }

                    var existingPermIds = role.Permissions.Select(p => p.Id).ToHashSet();
                    foreach (var perm in minimalPermissions.Where(p => !existingPermIds.Contains(p.Id)))
                    {
                        role.Permissions.Add(perm);
                    }

                    if (toRemove.Count > 0 || minimalPermissions.Any(p => !existingPermIds.Contains(p.Id)))
                    {
                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    var targetPermIds = GetPermissionsForRole(rCode).ToHashSet();
                    if (targetPermIds.Count > 0)
                    {
                        var existingPermIds = role.Permissions.Select(p => p.Id).ToHashSet();
                        var missing = allPermissions
                            .Where(p => targetPermIds.Contains(p.Id) && !existingPermIds.Contains(p.Id))
                            .ToList();

                        if (missing.Count > 0)
                        {
                            foreach (var perm in missing)
                            {
                                role.Permissions.Add(perm);
                            }
                            await context.SaveChangesAsync();
                        }
                    }
                }
            }

            var docType = await context.Set<DocumentTypeEntity>().FirstOrDefaultAsync();
            var docTypeId = docType?.Id ?? new Guid("e0000000-0000-0000-0000-000000000001");

            var personsToEnsure = new (Guid Id, string DocNum, string First, string Last, string Email)[]
            {
                (AdminUserSeeder.AdminPersonId, "1234567890", "Carlos", "Administrator", "admin@nexusodonto.com"),
                (AdminUserSeeder.DentistPersonId, "1098765432", "Laura", "Gómez", "odontologo@nexusodonto.com"),
                (AdminUserSeeder.ReceptionistPersonId, "1087654321", "María", "Rodríguez", "recepcion@nexusodonto.com"),
                (AdminUserSeeder.PatientPersonId, "1076543210", "Andrés", "Pérez", "paciente@nexusodonto.com"),
                (UserSeeder.BotPersonId, "BOT-SERVICE-01", "Chatbot", "Service", "bot_service@nexusodonto.com"),
                (new Guid("1b000000-0000-0000-0000-000000000010"), "1098765433", "Roberto", "Martínez", "roberto.martinez@nexusodonto.com"),
                (new Guid("1b000000-0000-0000-0000-000000000011"), "1098765434", "Ana Sofía", "Silva", "ana.silva@nexusodonto.com"),
                (new Guid("1b000000-0000-0000-0000-000000000013"), "1076543211", "Camila", "López", "camila.lopez@gmail.com"),
                (new Guid("1b000000-0000-0000-0000-000000000014"), "1076543212", "Felipe", "Torres", "felipe.torres@hotmail.com"),
                (new Guid("1b000000-0000-0000-0000-000000000015"), "1076543213", "Valentina", "Mendoza", "valentina.mendoza@yahoo.com")
            };

            foreach (var pInfo in personsToEnsure)
            {
                var person = await context.Set<PersonEntity>().FirstOrDefaultAsync(p => p.Id == pInfo.Id || p.DocumentNumber == pInfo.DocNum || p.Email == pInfo.Email);
                if (person is null)
                {
                    person = new PersonEntity(docTypeId, pInfo.DocNum, pInfo.First, pInfo.Last, email: pInfo.Email, address: "NexusOdonto Clinic");
                    SetEntityId(person, pInfo.Id);
                    await context.Set<PersonEntity>().AddAsync(person);
                    await context.SaveChangesAsync();
                }
                else if (pInfo.Id == AdminUserSeeder.AdminPersonId && (person.FirstName != pInfo.First || person.Email != pInfo.Email))
                {
                    person.Update(pInfo.First, pInfo.Last, person.DateOfBirth, person.SexId, person.Phone, pInfo.Email, person.Address);
                    context.Set<PersonEntity>().Update(person);
                    await context.SaveChangesAsync();
                }
            }

            var usersToEnsure = new (Guid UserId, Guid PersonId, string Password, Guid RoleId)[]
            {
                (AdminUserSeeder.AdminUserId, AdminUserSeeder.AdminPersonId, "Admin123!", RoleSeeder.Administrator),
                (AdminUserSeeder.DentistUserId, AdminUserSeeder.DentistPersonId, "Doctor123!", RoleSeeder.Dentist),
                (AdminUserSeeder.Dentist2UserId, AdminUserSeeder.Dentist2PersonId, "Doctor123!", RoleSeeder.Dentist),
                (AdminUserSeeder.Dentist3UserId, AdminUserSeeder.Dentist3PersonId, "Doctor123!", RoleSeeder.Dentist),
                (AdminUserSeeder.ReceptionistUserId, AdminUserSeeder.ReceptionistPersonId, "Recepcion123!", RoleSeeder.Receptionist),
                (AdminUserSeeder.PatientUserId, AdminUserSeeder.PatientPersonId, "Paciente123!", RoleSeeder.Patient),
                (AdminUserSeeder.Patient2UserId, AdminUserSeeder.Patient2PersonId, "Paciente123!", RoleSeeder.Patient),
                (AdminUserSeeder.Patient3UserId, AdminUserSeeder.Patient3PersonId, "Paciente123!", RoleSeeder.Patient),
                (AdminUserSeeder.Patient4UserId, AdminUserSeeder.Patient4PersonId, "Paciente123!", RoleSeeder.Patient),
                (UserSeeder.BotUserId, UserSeeder.BotPersonId, "BotService2026!", RoleSeeder.BotService)
            };

            foreach (var uInfo in usersToEnsure)
            {
                var user = await context.Set<UserEntity>().FirstOrDefaultAsync(u => u.Id == uInfo.UserId || u.PersonId == uInfo.PersonId);
                if (user is null)
                {
                    var hash = BCrypt.Net.BCrypt.HashPassword(uInfo.Password, 11);
                    user = new UserEntity(uInfo.PersonId, hash);
                    SetEntityId(user, uInfo.UserId);
                    await context.Set<UserEntity>().AddAsync(user);
                    await context.SaveChangesAsync();
                }
                else
                {
                    if (!BCrypt.Net.BCrypt.Verify(uInfo.Password, user.PasswordHash))
                    {
                        var newHash = BCrypt.Net.BCrypt.HashPassword(uInfo.Password, 11);
                        user.Update(newHash, user.LastAccessAt);
                        context.Set<UserEntity>().Update(user);
                        await context.SaveChangesAsync();
                    }
                }

                var ur = await context.Set<UserRoleEntity>().FirstOrDefaultAsync(r => r.UserId == user.Id && r.RoleId == uInfo.RoleId);
                if (ur is null)
                {
                    ur = new UserRoleEntity(user.Id, uInfo.RoleId);
                    await context.Set<UserRoleEntity>().AddAsync(ur);
                    await context.SaveChangesAsync();
                }
            }

            var empJobAdmin = new Guid("11000000-0000-0000-0000-000000000001");
            var empJobRecep = new Guid("11000000-0000-0000-0000-000000000002");
            var empJobDent = new Guid("11000000-0000-0000-0000-000000000003");
            var empJobAssist = new Guid("11000000-0000-0000-0000-000000000004");

            var employeesToEnsure = new (Guid EmpId, Guid PersonId, Guid JobId, string EmployeeCode)[]
            {
                (EmployeeSeeder.AdminEmployeeId, AdminUserSeeder.AdminPersonId, empJobAdmin, "AD001"),
                (EmployeeSeeder.Dentist1EmployeeId, AdminUserSeeder.DentistPersonId, empJobDent, "OD001"),
                (EmployeeSeeder.Dentist2EmployeeId, AdminUserSeeder.Dentist2PersonId, empJobDent, "OD002"),
                (EmployeeSeeder.Dentist3EmployeeId, AdminUserSeeder.Dentist3PersonId, empJobDent, "OD003"),
                (EmployeeSeeder.ReceptionistEmployeeId, AdminUserSeeder.ReceptionistPersonId, empJobRecep, "RC001"),
                (EmployeeSeeder.BotEmployeeId, UserSeeder.BotPersonId, empJobAssist, "BOT001")
            };

            foreach (var eInfo in employeesToEnsure)
            {
                var emp = await context.Set<EmployeeEntity>().FirstOrDefaultAsync(e => e.Id == eInfo.EmpId || e.PersonId == eInfo.PersonId);
                if (emp is null)
                {
                    emp = new EmployeeEntity(eInfo.PersonId, eInfo.JobId, new DateOnly(2026, 1, 15), eInfo.EmployeeCode);
                    SetEntityId(emp, eInfo.EmpId);
                    await context.Set<EmployeeEntity>().AddAsync(emp);
                    await context.SaveChangesAsync();
                }
                else if (!string.Equals(emp.EmployeeCode, eInfo.EmployeeCode, StringComparison.Ordinal))
                {
                    emp.Update(eInfo.JobId, emp.HireDate, eInfo.EmployeeCode);
                    context.Set<EmployeeEntity>().Update(emp);
                    await context.SaveChangesAsync();
                }
            }

            var professionalsToEnsure = new (Guid ProfId, Guid EmpId, string License)[]
            {
                (ProfessionalSeeder.Dentist1ProfessionalId, EmployeeSeeder.Dentist1EmployeeId, "TP-OD-1098765"),
                (ProfessionalSeeder.Dentist2ProfessionalId, EmployeeSeeder.Dentist2EmployeeId, "TP-OD-1098766"),
                (ProfessionalSeeder.Dentist3ProfessionalId, EmployeeSeeder.Dentist3EmployeeId, "TP-OD-1098767")
            };

            var professionalSpecialties = new (Guid ProfId, Guid[] SpecialtyIds)[]
            {
                (ProfessionalSeeder.Dentist1ProfessionalId,
                [
                    new Guid("18000000-0000-0000-0000-000000000001"),
                    new Guid("18000000-0000-0000-0000-000000000002")
                ]),
                (ProfessionalSeeder.Dentist2ProfessionalId,
                [
                    new Guid("18000000-0000-0000-0000-000000000003"),
                    new Guid("18000000-0000-0000-0000-000000000006")
                ]),
                (ProfessionalSeeder.Dentist3ProfessionalId,
                [
                    new Guid("18000000-0000-0000-0000-000000000004"),
                    new Guid("18000000-0000-0000-0000-000000000005")
                ])
            };

            foreach (var profInfo in professionalsToEnsure)
            {
                var prof = await context.Set<ProfessionalEntity>()
                    .Include(p => p.Specialties)
                    .FirstOrDefaultAsync(p => p.Id == profInfo.ProfId || p.EmployeeId == profInfo.EmpId);

                if (prof is null)
                {
                    prof = new ProfessionalEntity(profInfo.EmpId, profInfo.License);
                    SetEntityId(prof, profInfo.ProfId);
                    await context.Set<ProfessionalEntity>().AddAsync(prof);
                    await context.SaveChangesAsync();
                }

                var mapping = professionalSpecialties.First(m => m.ProfId == profInfo.ProfId);
                var existingSpecialtyIds = prof.Specialties.Select(s => s.Id).ToHashSet();
                var missingSpecialtyIds = mapping.SpecialtyIds.Where(id => !existingSpecialtyIds.Contains(id)).ToList();

                if (missingSpecialtyIds.Count > 0)
                {
                    var specialties = await context.Set<SpecialtyEntity>()
                        .Where(s => missingSpecialtyIds.Contains(s.Id))
                        .ToListAsync();

                    foreach (var specialty in specialties)
                    {
                        prof.Specialties.Add(specialty);
                    }

                    await context.SaveChangesAsync();
                }
            }

            var patientsToEnsure = new (Guid PatId, Guid PersonId, string Contact, string Phone)[]
            {
                (PatientSeeder.Patient1Id, AdminUserSeeder.PatientPersonId, "Lucía Pérez", "+573101234567"),
                (PatientSeeder.Patient2Id, new Guid("1b000000-0000-0000-0000-000000000013"), "Jorge López", "+573112345678"),
                (PatientSeeder.Patient3Id, new Guid("1b000000-0000-0000-0000-000000000014"), "Marta Torres", "+573123456789"),
                (PatientSeeder.Patient4Id, new Guid("1b000000-0000-0000-0000-000000000015"), "Carlos Mendoza", "+573134567890")
            };

            foreach (var patInfo in patientsToEnsure)
            {
                var pat = await context.Set<PatientEntity>().FirstOrDefaultAsync(p => p.Id == patInfo.PatId || p.PersonId == patInfo.PersonId);
                if (pat is null)
                {
                    pat = new PatientEntity(patInfo.PersonId, patInfo.Contact, patInfo.Phone);
                    SetEntityId(pat, patInfo.PatId);
                    await context.Set<PatientEntity>().AddAsync(pat);
                    await context.SaveChangesAsync();
                }
            }

            var availabilitiesToEnsure = new (Guid AvailId, Guid ProfId, int StartDay, int EndDay, TimeOnly StartTime, TimeOnly LunchStart, TimeOnly LunchEnd, TimeOnly EndTime)[]
            {
                (new Guid("1f000000-0000-0000-0000-000000000001"), ProfessionalSeeder.Dentist1ProfessionalId, 1, 5, new TimeOnly(8, 0), new TimeOnly(12, 0), new TimeOnly(13, 0), new TimeOnly(17, 0)),
                (new Guid("1f000000-0000-0000-0000-000000000002"), ProfessionalSeeder.Dentist1ProfessionalId, 6, 6, new TimeOnly(8, 0), new TimeOnly(12, 0), new TimeOnly(12, 0), new TimeOnly(12, 0)),
                (new Guid("1f000000-0000-0000-0000-000000000003"), ProfessionalSeeder.Dentist2ProfessionalId, 1, 5, new TimeOnly(7, 0), new TimeOnly(12, 0), new TimeOnly(13, 0), new TimeOnly(16, 0)),
                (new Guid("1f000000-0000-0000-0000-000000000004"), ProfessionalSeeder.Dentist3ProfessionalId, 1, 5, new TimeOnly(9, 0), new TimeOnly(13, 0), new TimeOnly(14, 0), new TimeOnly(18, 0))
            };

            foreach (var avInfo in availabilitiesToEnsure)
            {
                var av = await context.Set<AvailabilityEntity>().FirstOrDefaultAsync(a => a.Id == avInfo.AvailId);
                if (av is null)
                {
                    av = new AvailabilityEntity(avInfo.ProfId, avInfo.StartDay, avInfo.EndDay, avInfo.StartTime, avInfo.LunchStart, avInfo.LunchEnd, avInfo.EndTime);
                    SetEntityId(av, avInfo.AvailId);
                    await context.Set<AvailabilityEntity>().AddAsync(av);
                    await context.SaveChangesAsync();
                }
            }

            logger?.LogInformation("DatabaseInitializer: Todos los seeders iniciales (Personas, Usuarios, Empleados, Doctores, Pacientes, Horarios y Permisos Bot) han sido verificados e insertados en la base de datos.");
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "Error ejecutando el inicializador de datos de la base de datos.");
        }
    }

    private static List<Guid> GetPermissionsForRole(string roleCode)
    {
        var result = new List<Guid>();
        void Add(int m, int a) => result.Add(new Guid($"14000000-0000-0000-{m:D4}-{a:D12}"));

        if (roleCode == "ODONTOLOGO")
        {
            int[] dentistModules = [1, 2, 3, 5, 6, 7, 8, 10];
            foreach (var m in dentistModules)
            {
                Add(m, 1);
                Add(m, 2);
                Add(m, 3);
                if (m is 2 or 5 or 6) Add(m, 4);
            }
        }
        else if (roleCode == "RECEPCIONISTA")
        {
            int[] receptionistModules = [1, 2, 3, 5, 7, 8, 10];
            foreach (var m in receptionistModules)
            {
                Add(m, 1);
                if (m is 1 or 2 or 7 or 8 or 10)
                {
                    Add(m, 2);
                    Add(m, 3);
                }
                if (m is 1 or 2) Add(m, 4);
            }
        }
        else if (roleCode == "ASISTENTE")
        {
            int[] assistantModules = [1, 2, 3, 5, 6, 7, 10];
            foreach (var m in assistantModules) Add(m, 1);
        }
        else if (roleCode == "PACIENTE")
        {
            int[] patientModules = [1, 2, 3, 7, 8];
            foreach (var m in patientModules)
            {
                Add(m, 1);
                if (m is 1 or 2 or 8)
                {
                    Add(m, 2);
                    Add(m, 3);
                }
            }
        }
        return result;
    }

    private static void SetEntityId<T>(T entity, Guid id) where T : class
    {
        var prop = typeof(T).GetProperty("Id");
        if (prop != null && prop.CanWrite)
        {
            prop.SetValue(entity, id);
        }
    }
}
