using Domain.Entities.People;
using Domain.Entities.Security;
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
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetService<ILogger<AppDbContext>>();

        try
        {
            // 1. Verificar o crear Rol BOT_SERVICE
            var botRole = await context.Set<RoleEntity>().FirstOrDefaultAsync(r => r.Id == RoleSeeder.BotService || r.Code == "BOT_SERVICE");
            if (botRole is null)
            {
                botRole = new RoleEntity("BOT_SERVICE", "Bot Service", "Service role for automated Chatbot integration.");
                SetEntityId(botRole, RoleSeeder.BotService);
                await context.Set<RoleEntity>().AddAsync(botRole);
                await context.SaveChangesAsync();
            }

            // 2. Verificar o crear Persona Bot
            var botPerson = await context.Set<PersonEntity>().FirstOrDefaultAsync(p => p.Email == "bot_service@nexusodonto.com" || p.Id == UserSeeder.BotPersonId);
            if (botPerson is null)
            {
                var docType = await context.Set<Domain.Entities.Catalogs.DocumentTypeEntity>().FirstOrDefaultAsync();
                var docTypeId = docType?.Id ?? new Guid("e0000000-0000-0000-0000-000000000001");

                botPerson = new PersonEntity(
                    documentTypeId: docTypeId,
                    documentNumber: "BOT-SERVICE-01",
                    firstName: "Chatbot",
                    lastName: "Service",
                    email: "bot_service@nexusodonto.com",
                    address: "Sistema Interno NexusOdonto");

                SetEntityId(botPerson, UserSeeder.BotPersonId);
                await context.Set<PersonEntity>().AddAsync(botPerson);
                await context.SaveChangesAsync();
            }

            // 3. Verificar o crear Usuario Bot con Hash válido para 'BotService2026!'
            const string validHash = "$2a$11$7exqDQzqUbvt70Ph/7ySpev0OU8YKHaW81fEbDdAyhlUqRKD0Y6mW";
            var botUser = await context.Set<UserEntity>().FirstOrDefaultAsync(u => u.PersonId == botPerson.Id || u.Id == UserSeeder.BotUserId);

            if (botUser is null)
            {
                botUser = new UserEntity(botPerson.Id, validHash);
                SetEntityId(botUser, UserSeeder.BotUserId);
                await context.Set<UserEntity>().AddAsync(botUser);
                await context.SaveChangesAsync();
            }
            else
            {
                // Si la contraseña no coincide con 'BotService2026!', actualizar el hash
                if (!BCrypt.Net.BCrypt.Verify("BotService2026!", botUser.PasswordHash))
                {
                    botUser.Update(validHash, botUser.LastAccessAt);
                    context.Set<UserEntity>().Update(botUser);
                    await context.SaveChangesAsync();
                }
            }

            // 4. Verificar o crear relación UserRole
            var userRole = await context.Set<UserRoleEntity>().FirstOrDefaultAsync(ur => ur.UserId == botUser.Id && ur.RoleId == botRole.Id);
            if (userRole is null)
            {
                userRole = new UserRoleEntity(botUser.Id, botRole.Id);
                await context.Set<UserRoleEntity>().AddAsync(userRole);
                await context.SaveChangesAsync();
            }

            // 5. Asignar TODOS los permisos al rol BOT_SERVICE (acceso completo estilo Administrador)
            var allPermissions = await context.Set<PermissionEntity>().ToListAsync();
            var existingPermissionIds = await context.Database
                .SqlQueryRaw<Guid>("SELECT permission_id FROM role_permissions WHERE role_id = {0}", botRole.Id)
                .ToListAsync();

            var missingPermissions = allPermissions
                .Where(p => !existingPermissionIds.Contains(p.Id))
                .ToList();

            if (missingPermissions.Any())
            {
                foreach (var perm in missingPermissions)
                {
                    await context.Database.ExecuteSqlRawAsync(
                        "INSERT INTO role_permissions (role_id, permission_id) VALUES ({0}, {1})",
                        botRole.Id, perm.Id);
                }
            }

            logger?.LogInformation("Usuario bot_service@nexusodonto.com verificado con TODOS los permisos de administrador en la base de datos.");
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "Error inicializando el usuario bot_service@nexusodonto.com y sus permisos en la base de datos.");
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
