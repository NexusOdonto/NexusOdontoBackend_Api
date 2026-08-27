using HubNotificationStatusEntity = Domain.Entities.Catalogs.HubNotificationStatus;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.HubNotificationStatus;

public static class HubNotificationStatusSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<HubNotificationStatusEntity>().HasData(
            new
            {
                Id = new Guid("60000000-0000-0000-0000-000000000001"),
                Code = "PENDIENTE",
                Name = "Pending",
                Description = "The notification is waiting to be delivered.",
                IsActive = true,
                SortOrder = (int?)1,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("60000000-0000-0000-0000-000000000002"),
                Code = "ENTREGADA",
                Name = "Delivered",
                Description = "The notification was delivered to the recipient.",
                IsActive = true,
                SortOrder = (int?)2,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("60000000-0000-0000-0000-000000000003"),
                Code = "LEIDA",
                Name = "Read",
                Description = "The recipient has read the notification.",
                IsActive = true,
                SortOrder = (int?)3,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("60000000-0000-0000-0000-000000000004"),
                Code = "CANCELADA",
                Name = "Cancelled",
                Description = "The notification was cancelled before delivery.",
                IsActive = true,
                SortOrder = (int?)4,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}

