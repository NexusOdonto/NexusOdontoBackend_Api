using Application.Contracts.Repositories.Common;
using Domain.Entities.Chatbot;

namespace Application.Contracts.Repositories.Chatbot;

public interface IChatbotConversationRepository : IRepository<ChatbotConversationEntity>
{
    Task<ChatbotConversationEntity?> GetByChannelAndChatIdAsync(
        Guid chatChannelId,
        string chatIdentifier,
        CancellationToken cancellationToken = default);

    Task<ChatbotConversationEntity?> GetLatestByPatientIdAsync(
        Guid patientId,
        CancellationToken cancellationToken = default);
}
