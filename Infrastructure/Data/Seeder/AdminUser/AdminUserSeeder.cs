using Domain.Entities.People;
using Domain.Entities.Security;
using Infrastructure.Data.Seeder.Role;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.AdminUser;

/// <summary>
/// Development-only administrator account.
/// Credentials (dev only, never use in production):
///   Email: admin@nexus.local
///   Document: 1234567890
///   Password: Admin123!
/// </summary>
public static class AdminUserSeeder
{
    public static readonly Guid PersonId = new("1b000000-0000-0000-0000-000000000001");
    public static readonly Guid UserId = new("1b000000-0000-0000-0000-000000000002");

    // BCrypt hash of "Admin123!" (work factor 11).
    private const string PasswordHash =
        "$2a$11$20hYXWKcqvIPrfsNyCCB1eJJZgRWoTXIA2JWITxYKRY3KJajuY0Aq";

    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var assignedAt = seedCreatedAt;

        modelBuilder.Entity<PersonEntity>().HasData(
            new
            {
                Id = PersonId,
                DocumentTypeId = new Guid("e0000000-0000-0000-0000-000000000001"),
                DocumentNumber = "1234567890",
                FirstName = "Admin",
                LastName = "Sistema",
                DateOfBirth = (DateTime?)null,
                SexId = (Guid?)null,
                Phone = (string?)null,
                Email = "admin@nexus.local",
                Address = (string?)null,
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });

        modelBuilder.Entity<UserEntity>().HasData(
            new
            {
                Id = UserId,
                PersonId,
                PasswordHash,
                LastAccessAt = (DateTime?)null,
                IsActive = true,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });

        modelBuilder.Entity<UserRoleEntity>().HasData(
            new
            {
                UserId,
                RoleId = RoleSeeder.Administrator,
                AssignedAt = assignedAt
            });
    }
}
