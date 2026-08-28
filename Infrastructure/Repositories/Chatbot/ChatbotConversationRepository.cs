using Application.Contracts.Repositories.Chatbot;
using Domain.Entities.Chatbot;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Chatbot;

public sealed class ChatbotConversationRepository : Common.Repository<ChatbotConversationEntity>, IChatbotConversationRepository
{
    public ChatbotConversationRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<ChatbotConversationEntity?> GetByChannelAndChatIdAsync(
        Guid chatChannelId,
        string chatIdentifier,
        CancellationToken cancellationToken = default)
    {
        return await Set.FirstOrDefaultAsync(
            c => c.ChatChannelId == chatChannelId && c.ChatIdentifier == chatIdentifier,
            cancellationToken);
    }
}
