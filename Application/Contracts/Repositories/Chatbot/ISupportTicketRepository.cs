using Application.Contracts.Repositories.Common;
using Domain.Entities.Chatbot;

namespace Application.Contracts.Repositories.Chatbot;

public interface ISupportTicketRepository : IRepository<SupportTicketEntity>
{
    Task<SupportTicketEntity?> GetOpenByConversationIdAsync(
        Guid chatbotConversationId,
        CancellationToken cancellationToken = default);
}
