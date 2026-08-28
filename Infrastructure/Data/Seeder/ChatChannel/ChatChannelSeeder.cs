using ChatChannelEntity = Domain.Entities.Catalogs.ChatChannelEntity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.ChatChannel;

public static class ChatChannelSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<ChatChannelEntity>().HasData(
            new
            {
                Id = new Guid("90000000-0000-0000-0000-000000000001"),
                Code = "WHATSAPP",
                Name = "WhatsApp",
                Description = "Conversation initiated through the WhatsApp channel.",
                IsActive = true,
                SortOrder = (int?)1,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("90000000-0000-0000-0000-000000000002"),
                Code = "TELEGRAM",
                Name = "Telegram",
                Description = "Conversation initiated through the Telegram channel.",
                IsActive = true,
                SortOrder = (int?)2,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("90000000-0000-0000-0000-000000000003"),
                Code = "WEBCHAT",
                Name = "Web Chat",
                Description = "Conversation initiated through the web chat channel.",
                IsActive = true,
                SortOrder = (int?)3,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}

