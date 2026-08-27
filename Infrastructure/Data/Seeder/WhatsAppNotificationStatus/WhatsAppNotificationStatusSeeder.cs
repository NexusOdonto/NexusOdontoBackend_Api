using WhatsAppNotificationStatusEntity = Domain.Entities.Catalogs.WhatsAppNotificationStatus;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.WhatsAppNotificationStatus;

public static class WhatsAppNotificationStatusSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<WhatsAppNotificationStatusEntity>().HasData(
            new
            {
                Id = new Guid("80000000-0000-0000-0000-000000000001"),
                Code = "PENDIENTE",
                Name = "Pending",
                Description = "The message is waiting to be sent.",
                IsActive = true,
                SortOrder = (int?)1,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("80000000-0000-0000-0000-000000000002"),
                Code = "ENVIADA",
                Name = "Sent",
                Description = "The message was sent to the messaging provider.",
                IsActive = true,
                SortOrder = (int?)2,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("80000000-0000-0000-0000-000000000003"),
                Code = "ENTREGADA",
                Name = "Delivered",
                Description = "The message was delivered to the recipient device.",
                IsActive = true,
                SortOrder = (int?)3,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("80000000-0000-0000-0000-000000000004"),
                Code = "LEIDA",
                Name = "Read",
                Description = "The recipient has read the message.",
                IsActive = true,
                SortOrder = (int?)4,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("80000000-0000-0000-0000-000000000005"),
                Code = "FALLIDA",
                Name = "Failed",
                Description = "The message could not be delivered due to an error.",
                IsActive = true,
                SortOrder = (int?)5,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}

