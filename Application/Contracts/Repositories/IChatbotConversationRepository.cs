using Domain.Entities.Chatbot;

namespace Application.Contracts.Repositories;

public interface IChatbotConversationRepository : IRepository<ChatbotConversation>
{
    Task<ChatbotConversation?> GetByChannelAndChatIdAsync(
        Guid chatChannelId,
        string chatIdentifier,
        CancellationToken cancellationToken = default);
}
