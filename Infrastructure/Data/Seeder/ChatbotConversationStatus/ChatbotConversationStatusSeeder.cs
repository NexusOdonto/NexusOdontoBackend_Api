using ChatbotConversationStatusEntity = Domain.Entities.Catalogs.ChatbotConversationStatus;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.ChatbotConversationStatus;

public static class ChatbotConversationStatusSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var seedCreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<ChatbotConversationStatusEntity>().HasData(
            new
            {
                Id = new Guid("b0000000-0000-0000-0000-000000000001"),
                Code = "ACTIVA",
                Name = "Active",
                Description = "The conversation is active and being handled by the chatbot.",
                IsFinal = false,
                IsActive = true,
                SortOrder = (int?)1,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("b0000000-0000-0000-0000-000000000002"),
                Code = "ESCALADA",
                Name = "Escalated",
                Description = "The conversation was escalated and is waiting for human support.",
                IsFinal = false,
                IsActive = true,
                SortOrder = (int?)2,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("b0000000-0000-0000-0000-000000000003"),
                Code = "ATENDIDA_HUMANO",
                Name = "Handled by Human",
                Description = "A human agent is currently handling the conversation.",
                IsFinal = false,
                IsActive = true,
                SortOrder = (int?)3,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            },
            new
            {
                Id = new Guid("b0000000-0000-0000-0000-000000000004"),
                Code = "CERRADA",
                Name = "Closed",
                Description = "The conversation has ended and requires no further action.",
                IsFinal = true,
                IsActive = true,
                SortOrder = (int?)4,
                CreatedAt = seedCreatedAt,
                UpdatedAt = (DateTime?)null
            });
    }
}

