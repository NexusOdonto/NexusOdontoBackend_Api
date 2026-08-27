using Application.Contracts.Repositories;
using Application.Contracts.Services.Chatbot;
using Application.DTOs.Support.SupportTicket;
using Domain.Entities.Catalogs;
using Domain.Entities.Chatbot;

namespace Application.Services.Chatbot;

public sealed class SupportTicketService(
    ISupportTicketRepository repository,
    ICatalogRepository<SupportTicketStatus> statusCatalog,
    IUnitOfWork unitOfWork) : ISupportTicketService
{
    public async Task<IReadOnlyList<object>> GetAllAsync()
    {
        var items = await repository.GetAllAsync();
        return items.Select(Map).Cast<object>().ToList();
    }

    public async Task<object?> GetByIdAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<object> CreateAsync(CreateSupportTicketRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var statusId = await ServiceWrite.ResolveCatalogIdAsync(statusCatalog, "OPEN", "NEW", "PENDING");

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new SupportTicket(
                request.ChatbotConversationId,
                request.TicketReasonId,
                request.NotificationPriorityId,
                statusId,
                request.Title,
                request.PatientId,
                request.Description,
                request.ChatbotSummary,
                request.RagConfidence);
            await repository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<object?> UpdateAsync(Guid id, UpdateSupportTicketRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.TicketReasonId = request.TicketReasonId;
            entity.Title = request.Title;
            entity.Description = request.Description;
            var assignedAt = request.AssignedToUserId.HasValue ? DateTime.UtcNow : entity.AssignedAt;
            entity.Update(
                request.TicketStatusId,
                request.NotificationPriorityId,
                request.AssignedToUserId,
                assignedAt,
                entity.AttendedAt,
                entity.ResolvedByUserId,
                entity.ResolvedAt,
                request.Resolution,
                entity.ClosedAt);
            repository.Update(entity);
            return Map(entity);
        });
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return false;
        await ServiceWrite.InTransactionAsync(unitOfWork, () => { repository.Remove(entity); return Task.CompletedTask; });
        return true;
    }

    private static object Map(SupportTicket e) => new
    {
        e.Id, e.ChatbotConversationId, e.PatientId, e.TicketReasonId, e.NotificationPriorityId,
        e.TicketStatusId, e.Title, e.Description, e.ChatbotSummary, e.RagConfidence,
        e.CreatedAt, e.AssignedToUserId, e.AssignedAt, e.Resolution, e.ClosedAt
    };
}
