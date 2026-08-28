using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.Catalogs;
using Application.Contracts.Repositories.Chatbot;
using Application.Contracts.Services.Chatbot;
using Application.DTOs.Support.SupportTicket;
using Domain.Entities.Catalogs;
using Domain.Entities.Chatbot;

namespace Application.Services.Chatbot;

public sealed class SupportTicketService(
    ISupportTicketRepository repository,
    ICatalogRepository<SupportTicketStatusEntity> statusCatalog,
    IUnitOfWork unitOfWork) : ISupportTicketService
{
    public async Task<IReadOnlyList<SupportTicketResponseDto>> GetAllAsync()
    {
        var items = await repository.GetAllAsync();
        return items.Select(Map).ToList();
    }

    public async Task<SupportTicketResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<SupportTicketResponseDto> CreateAsync(CreateSupportTicketRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var statusId = await ServiceWrite.ResolveCatalogIdAsync(statusCatalog, "OPEN", "NEW", "PENDING");

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new SupportTicketEntity(
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

    public async Task<SupportTicketResponseDto?> UpdateAsync(Guid id, UpdateSupportTicketRequestDto request)
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

    private static SupportTicketResponseDto Map(SupportTicketEntity e) => new()
    {
        Id = e.Id,
        ChatbotConversationId = e.ChatbotConversationId,
        PatientId = e.PatientId,
        TicketReasonId = e.TicketReasonId,
        NotificationPriorityId = e.NotificationPriorityId,
        TicketStatusId = e.TicketStatusId,
        Title = e.Title,
        Description = e.Description,
        ChatbotSummary = e.ChatbotSummary,
        RagConfidence = e.RagConfidence,
        CreatedAt = e.CreatedAt,
        AssignedToUserId = e.AssignedToUserId,
        AssignedAt = e.AssignedAt,
        Resolution = e.Resolution,
        ClosedAt = e.ClosedAt
    };
}
