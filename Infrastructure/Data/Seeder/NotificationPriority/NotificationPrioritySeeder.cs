using NotificationPriorityEntity = Domain.Entities.Catalogs.NotificationPriorityEntity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.NotificationPriority;

public static class NotificationPrioritySeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<NotificationPriorityEntity>().HasData(
            new
            {
                Id = new Guid("50000000-0000-0000-0000-000000000001"),
                Code = "BAJA",
                Name = "Low",
                Description = "Low-priority information that does not require immediate action.",
                ColorHex = "#22C55E",
                Level = (int?)1,
                IsActive = true,
                SortOrder = (int?)1,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("50000000-0000-0000-0000-000000000002"),
                Code = "NORMAL",
                Name = "Normal",
                Description = "Standard-priority information for regular follow-up.",
                ColorHex = "#3B82F6",
                Level = (int?)2,
                IsActive = true,
                SortOrder = (int?)2,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("50000000-0000-0000-0000-000000000003"),
                Code = "ALTA",
                Name = "High",
                Description = "Important information that should be addressed promptly.",
                ColorHex = "#F59E0B",
                Level = (int?)3,
                IsActive = true,
                SortOrder = (int?)3,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("50000000-0000-0000-0000-000000000004"),
                Code = "URGENTE",
                Name = "Urgent",
                Description = "Critical information that requires immediate attention.",
                ColorHex = "#EF4444",
                Level = (int?)4,
                IsActive = true,
                SortOrder = (int?)4,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}

