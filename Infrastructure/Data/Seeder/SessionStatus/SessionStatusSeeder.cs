using SessionStatusEntity = Domain.Entities.Catalogs.SessionStatus;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.SessionStatus;

public static class SessionStatusSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<SessionStatusEntity>().HasData(
            new
            {
                Id = new Guid("15000000-0000-0000-0000-000000000001"),
                Code = "ACTIVA",
                Name = "Active",
                Description = "Session is currently valid.",
                IsActive = true,
                SortOrder = (int?)1,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("15000000-0000-0000-0000-000000000002"),
                Code = "REVOCADA",
                Name = "Revoked",
                Description = "Session was invalidated before its expiration.",
                IsActive = true,
                SortOrder = (int?)2,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("15000000-0000-0000-0000-000000000003"),
                Code = "EXPIRADA",
                Name = "Expired",
                Description = "Session reached its expiration time.",
                IsActive = true,
                SortOrder = (int?)3,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}
