using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    IChatbotConversationRepository conversationRepository,
    IRepository<ChatbotMessageEntity> messageRepository,
    ICatalogRepository<SupportTicketStatusEntity> statusCatalog,
    ICatalogRepository<ChatbotConversationStatusEntity> conversationStatusCatalog,
    ICatalogRepository<ChatbotMessageRoleEntity> roleCatalog,
    IChatbotClient chatbotClient,
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

    public async Task<ReplyTicketOutcome> ReplyToTicketAsync(Guid ticketId, string messageContent, string? messageRoleId = null, Guid? userId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(messageContent)) return ReplyTicketOutcome.NotFound;

        var ticket = await repository.GetByIdAsync(ticketId);
        if (ticket is null || ticket.ChatbotConversationId == Guid.Empty) return ReplyTicketOutcome.NotFound;

        var conversation = await conversationRepository.GetByIdAsync(ticket.ChatbotConversationId);
        if (conversation is null || string.IsNullOrWhiteSpace(conversation.ChatIdentifier)) return ReplyTicketOutcome.NotFound;

        Guid roleId;
        if (!string.IsNullOrWhiteSpace(messageRoleId) && Guid.TryParse(messageRoleId, out var parsedRoleId))
        {
            roleId = parsedRoleId;
        }
        else
        {
            roleId = await ServiceWrite.ResolveCatalogIdAsync(roleCatalog, "AGENTE_HUMANO", "HUMAN_AGENT", "ASESOR");
        }

        var trimmed = messageContent.Trim();

        // Send via FastAPI/Evolution first so the UI never gets a false success on WhatsApp failure.
        var delivered = await chatbotClient.SendAgentMessageAsync(conversation.ChatIdentifier, trimmed, cancellationToken);
        if (!delivered)
            return ReplyTicketOutcome.DeliveryFailed;

        await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var msgEntity = new ChatbotMessageEntity(
                conversation.Id,
                roleId,
                trimmed
            );
            await messageRepository.AddAsync(msgEntity);

            if (userId.HasValue)
            {
                ticket.AssignedToUserId = userId;
                ticket.AttendedAt ??= DateTime.UtcNow;
                repository.Update(ticket);
            }
        });

        return ReplyTicketOutcome.Success;
    }

    public async Task<bool> ResolveTicketAsync(
        Guid ticketId,
        string? resolution = null,
        Guid? userId = null,
        bool returnToBot = true,
        CancellationToken cancellationToken = default)
    {
        var ticket = await repository.GetByIdAsync(ticketId);
        if (ticket is null) return false;

        var resolvedStatusId = await ServiceWrite.ResolveCatalogIdAsync(statusCatalog, "RESOLVED", "RESUELTO", "CLOSED");
        var targetConvStatusId = returnToBot
            ? await ServiceWrite.ResolveCatalogIdAsync(conversationStatusCatalog, "ACTIVE", "ACTIVA")
            : await ServiceWrite.ResolveCatalogIdAsync(conversationStatusCatalog, "CLOSED", "CERRADA");

        if (ticket.ChatbotConversationId != Guid.Empty)
        {
            var conversation = await conversationRepository.GetByIdAsync(ticket.ChatbotConversationId);
            if (conversation is not null)
            {
                // Keep LastInteractionAt — status change alone should not reorder the chat list.
                var closedAt = returnToBot ? (DateTime?)null : DateTime.UtcNow;
                conversation.Update(targetConvStatusId, conversation.LastInteractionAt, closedAt, conversation.PatientId);
                conversationRepository.Update(conversation);
            }
        }

        await ServiceWrite.InTransactionAsync(unitOfWork, () =>
        {
            ticket.Update(
                resolvedStatusId,
                ticket.NotificationPriorityId,
                userId ?? ticket.AssignedToUserId,
                ticket.AssignedAt ?? DateTime.UtcNow,
                ticket.AttendedAt ?? DateTime.UtcNow,
                userId ?? ticket.ResolvedByUserId,
                DateTime.UtcNow,
                resolution ?? "Resuelto por asesor humano",
                DateTime.UtcNow
            );
            repository.Update(ticket);
            return Task.CompletedTask;
        });

        // WhatsApp "bot reactivado" notice: only FastAPI /conversations/resume (called once from front).
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
