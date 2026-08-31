using Domain.Entities.People;
using Domain.Entities.Security;
using Infrastructure.Data.Seeder.Role;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.User;

public static class UserSeeder
{
    public static readonly Guid BotPersonId = new("20000000-0000-0000-0000-000000000001");
    public static readonly Guid BotUserId = new("21000000-0000-0000-0000-000000000001");
    public const string BotDefaultPassword = "BotService2026!";

    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // 1. Persona para el servicio de Bot
        modelBuilder.Entity<PersonEntity>().HasData(new
        {
            Id = BotPersonId,
            DocumentTypeId = new Guid("e0000000-0000-0000-0000-000000000001"), // CC
            DocumentNumber = "BOT-SERVICE-01",
            FirstName = "Chatbot",
            LastName = "Service",
            Email = "bot_service@nexusodonto.com",
            SexId = (Guid?)new Guid("f0000000-0000-0000-0000-000000000003"), // OTRO
            Phone = "+573000000000",
            Address = "Sistema Interno NexusOdonto",
            IsActive = true,
            CreatedAt = seedCreatedAt,
            UpdatedAt = (DateTime?)null
        });

        // 2. Usuario exclusivo para el Bot (BCrypt hash de 'BotService2026!')
        modelBuilder.Entity<UserEntity>().HasData(new
        {
            Id = BotUserId,
            PersonId = BotPersonId,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(BotDefaultPassword, 11),
            LastAccessAt = (DateTime?)null,
            IsActive = true,
            CreatedAt = seedCreatedAt,
            UpdatedAt = (DateTime?)null
        });

        // 3. Asignación del rol BOT_SERVICE al usuario Bot
        modelBuilder.Entity<UserRoleEntity>().HasData(new
        {
            UserId = BotUserId,
            RoleId = RoleSeeder.BotService,
            AssignedAt = seedCreatedAt
        });
    }
}
