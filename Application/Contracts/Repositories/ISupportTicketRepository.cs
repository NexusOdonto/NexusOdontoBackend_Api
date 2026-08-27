using Domain.Entities.Chatbot;

namespace Application.Contracts.Repositories;

public interface ISupportTicketRepository : IRepository<SupportTicket>
{
    Task<SupportTicket?> GetOpenByConversationIdAsync(
        Guid chatbotConversationId,
        CancellationToken cancellationToken = default);
}
