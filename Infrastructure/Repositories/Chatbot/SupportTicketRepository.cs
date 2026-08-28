using Application.Contracts.Repositories.Chatbot;
using Domain.Entities.Chatbot;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Chatbot;

public sealed class SupportTicketRepository : Common.Repository<SupportTicketEntity>, ISupportTicketRepository
{
    public SupportTicketRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<SupportTicketEntity?> GetOpenByConversationIdAsync(
        Guid chatbotConversationId,
        CancellationToken cancellationToken = default)
    {
        return await Set
            .Where(t => t.ChatbotConversationId == chatbotConversationId && t.ClosedAt == null)
            .OrderByDescending(t => t.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
