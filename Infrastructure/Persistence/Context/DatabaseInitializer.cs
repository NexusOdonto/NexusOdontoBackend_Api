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
            // 1. Ensure Roles exist with full permissions for Admin and BotService
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

                if (rCode is "ADMINISTRADOR" or "BOT_SERVICE")
                {
                    var existingPermIds = role.Permissions.Select(p => p.Id).ToHashSet();
                    var missing = allPermissions.Where(p => !existingPermIds.Contains(p.Id)).ToList();
                    if (missing.Any())
                    {
                        foreach (var perm in missing)
                        {
                            role.Permissions.Add(perm);
                        }
                        await context.SaveChangesAsync();
                    }
                }
            }

            // 2. Ensure DocumentType
            var docType = await context.Set<Domain.Entities.Catalogs.DocumentTypeEntity>().FirstOrDefaultAsync();
            var docTypeId = docType?.Id ?? new Guid("e0000000-0000-0000-0000-000000000001");

            // 3. Ensure Persons
            var personsToEnsure = new (Guid Id, string DocNum, string First, string Last, string Email)[]
            {
                (AdminUserSeeder.AdminPersonId, "1234567890", "Carlos", "Administrator", "admin@nexusodonto.com"),
                (AdminUserSeeder.DentistPersonId, "1098765432", "Laura", "Gómez", "odontologo@nexusodonto.com"),
                (AdminUserSeeder.ReceptionistPersonId, "1087654321", "María", "Rodríguez", "recepcion@nexusodonto.com"),
                (AdminUserSeeder.PatientPersonId, "1076543210", "Andrés", "Pérez", "paciente@nexusodonto.com"),
                (UserSeeder.BotPersonId, "BOT-SERVICE-01", "Chatbot", "Service", "bot_service@nexusodonto.com"),
                (new Guid("1b000000-0000-0000-0000-000000000010"), "1098765433", "Roberto", "Martínez", "roberto.martinez@nexusodonto.com"),
                (new Guid("1b000000-0000-0000-0000-000000000011"), "1098765434", "Ana Sofía", "Silva", "ana.silva@nexusodonto.com"),
                (new Guid("1b000000-0000-0000-0000-000000000012"), "1087654322", "Juan", "Castro", "asistente@nexusodonto.com"),
                (new Guid("1b000000-0000-0000-0000-000000000013"), "1076543211", "Camila", "López", "camila.lopez@gmail.com"),
                (new Guid("1b000000-0000-0000-0000-000000000014"), "1076543212", "Felipe", "Torres", "felipe.torres@hotmail.com"),
                (new Guid("1b000000-0000-0000-0000-000000000015"), "1076543213", "Valentina", "Mendoza", "valentina.mendoza@yahoo.com")
            };

            foreach (var pInfo in personsToEnsure)
            {
                var person = await context.Set<PersonEntity>().FirstOrDefaultAsync(p => p.Id == pInfo.Id || p.Email == pInfo.Email);
                if (person is null)
                {
                    person = new PersonEntity(docTypeId, pInfo.DocNum, pInfo.First, pInfo.Last, email: pInfo.Email, address: "NexusOdonto Clinic");
                    SetEntityId(person, pInfo.Id);
                    await context.Set<PersonEntity>().AddAsync(person);
                    await context.SaveChangesAsync();
                }
            }

            // 4. Ensure Users and UserRoles
            var usersToEnsure = new (Guid UserId, Guid PersonId, string Password, Guid RoleId)[]
            {
                (AdminUserSeeder.AdminUserId, AdminUserSeeder.AdminPersonId, "Admin123!", RoleSeeder.Administrator),
                (AdminUserSeeder.DentistUserId, AdminUserSeeder.DentistPersonId, "Doctor123!", RoleSeeder.Dentist),
                (AdminUserSeeder.ReceptionistUserId, AdminUserSeeder.ReceptionistPersonId, "Recepcion123!", RoleSeeder.Receptionist),
                (AdminUserSeeder.PatientUserId, AdminUserSeeder.PatientPersonId, "Paciente123!", RoleSeeder.Patient),
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

            // 5. Ensure Employees
            var empJobAdmin = new Guid("11000000-0000-0000-0000-000000000001");
            var empJobRecep = new Guid("11000000-0000-0000-0000-000000000002");
            var empJobDent = new Guid("11000000-0000-0000-0000-000000000003");
            var empJobAssist = new Guid("11000000-0000-0000-0000-000000000004");

            var employeesToEnsure = new (Guid EmpId, Guid PersonId, Guid JobId)[]
            {
                (EmployeeSeeder.AdminEmployeeId, AdminUserSeeder.AdminPersonId, empJobAdmin),
                (EmployeeSeeder.Dentist1EmployeeId, AdminUserSeeder.DentistPersonId, empJobDent),
                (EmployeeSeeder.Dentist2EmployeeId, new Guid("1b000000-0000-0000-0000-000000000010"), empJobDent),
                (EmployeeSeeder.Dentist3EmployeeId, new Guid("1b000000-0000-0000-0000-000000000011"), empJobDent),
                (EmployeeSeeder.ReceptionistEmployeeId, AdminUserSeeder.ReceptionistPersonId, empJobRecep),
                (EmployeeSeeder.AssistantEmployeeId, new Guid("1b000000-0000-0000-0000-000000000012"), empJobAssist)
            };

            foreach (var eInfo in employeesToEnsure)
            {
                var emp = await context.Set<EmployeeEntity>().FirstOrDefaultAsync(e => e.Id == eInfo.EmpId || e.PersonId == eInfo.PersonId);
                if (emp is null)
                {
                    emp = new EmployeeEntity(eInfo.PersonId, eInfo.JobId, new DateOnly(2026, 1, 15));
                    SetEntityId(emp, eInfo.EmpId);
                    await context.Set<EmployeeEntity>().AddAsync(emp);
                    await context.SaveChangesAsync();
                }
            }

            // 6. Ensure Professionals
            var professionalsToEnsure = new (Guid ProfId, Guid EmpId, string License)[]
            {
                (ProfessionalSeeder.Dentist1ProfessionalId, EmployeeSeeder.Dentist1EmployeeId, "TP-OD-1098765"),
                (ProfessionalSeeder.Dentist2ProfessionalId, EmployeeSeeder.Dentist2EmployeeId, "TP-OD-1098766"),
                (ProfessionalSeeder.Dentist3ProfessionalId, EmployeeSeeder.Dentist3EmployeeId, "TP-OD-1098767")
            };

            foreach (var profInfo in professionalsToEnsure)
            {
                var prof = await context.Set<ProfessionalEntity>().FirstOrDefaultAsync(p => p.Id == profInfo.ProfId || p.EmployeeId == profInfo.EmpId);
                if (prof is null)
                {
                    prof = new ProfessionalEntity(profInfo.EmpId, profInfo.License);
                    SetEntityId(prof, profInfo.ProfId);
                    await context.Set<ProfessionalEntity>().AddAsync(prof);
                    await context.SaveChangesAsync();
                }
            }

            // 7. Ensure Patients
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

            // 8. Ensure Availabilities
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

    private static void SetEntityId<T>(T entity, Guid id) where T : class
    {
        var prop = typeof(T).GetProperty("Id");
        if (prop != null && prop.CanWrite)
        {
            prop.SetValue(entity, id);
        }
    }
}
