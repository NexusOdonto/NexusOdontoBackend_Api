using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.Catalogs;
using Application.Contracts.Repositories.Chatbot;
using Application.Contracts.Services.Chatbot;
using Application.DTOs.Chatbot.ChatbotConversation;
using Application.DTOs.Chatbot.ChatbotMessage;
using Domain.Entities.Catalogs;
using Domain.Entities.Chatbot;

namespace Application.Services.Chatbot;

public sealed class ChatbotConversationService(
    IChatbotConversationRepository repository,
    IRepository<ChatbotMessageEntity> messageRepository,
    ICatalogRepository<ChatbotConversationStatusEntity> statusCatalog,
    ICatalogRepository<ChatbotMessageRoleEntity> roleCatalog,
    IChatbotClient chatbotClient,
    IUnitOfWork unitOfWork) : IChatbotConversationService
{
    public async Task<IReadOnlyList<ChatbotConversationResponseDto>> GetAllAsync()
    {
        var items = await repository.GetAllAsync();
        return items.Select(Map).ToList();
    }

    public async Task<ChatbotConversationResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<ChatbotConversationResponseDto> CreateAsync(CreateChatbotConversationRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var statusId = await ServiceWrite.ResolveCatalogIdAsync(statusCatalog, "OPEN", "ACTIVE", "NEW");

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new ChatbotConversationEntity(
                request.ChatIdentifier,
                request.ChatChannelId,
                statusId,
                request.PatientId);
            await repository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<ChatbotConversationResponseDto?> UpdateAsync(Guid id, UpdateChatbotConversationRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.Update(
                request.ConversationStatusId,
                entity.LastInteractionAt,
                request.ClosedAt,
                request.PatientId);
            if (!string.IsNullOrWhiteSpace(request.ChatIdentifier))
                entity.SetChatIdentifier(request.ChatIdentifier);
            repository.Update(entity);
            return Map(entity);
        });
    }

    public async Task<IReadOnlyList<ChatbotMessageResponseDto>> GetMessagesAsync(Guid conversationId)
    {
        var allMessages = await messageRepository.GetAllAsync();
        return allMessages
            .Where(m => m.ChatbotConversationId == conversationId)
            .OrderBy(m => m.CreatedAt)
            .Select(m => new ChatbotMessageResponseDto
            {
                Id = m.Id,
                ChatbotConversationId = m.ChatbotConversationId,
                MessageRoleId = m.MessageRoleId,
                Content = m.Content,
                RagConfidence = m.RagConfidence,
                CreatedAt = m.CreatedAt
            })
            .ToList();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return false;
        await ServiceWrite.InTransactionAsync(unitOfWork, () => { repository.Remove(entity); return Task.CompletedTask; });
        return true;
    }

    public async Task<SendConversationWhatsAppOutcome> SendWhatsAppAsync(
        Guid conversationId,
        string messageContent,
        string? messageRoleId = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(messageContent))
            return SendConversationWhatsAppOutcome.NotFound;

        var conversation = await repository.GetByIdAsync(conversationId);
        if (conversation is null || string.IsNullOrWhiteSpace(conversation.ChatIdentifier))
            return SendConversationWhatsAppOutcome.NotFound;

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

        // Evolution first — never persist a false success if WhatsApp delivery fails.
        var delivered = await chatbotClient.SendAgentMessageAsync(conversation.ChatIdentifier, trimmed, cancellationToken);
        if (!delivered)
            return SendConversationWhatsAppOutcome.DeliveryFailed;

        await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var msgEntity = new ChatbotMessageEntity(
                conversation.Id,
                roleId,
                trimmed
            );
            await messageRepository.AddAsync(msgEntity);
        });

        return SendConversationWhatsAppOutcome.Success;
    }

    private static ChatbotConversationResponseDto Map(ChatbotConversationEntity e) => new()
    {
        Id = e.Id,
        PatientId = e.PatientId,
        ChatIdentifier = e.ChatIdentifier,
        ChatChannelId = e.ChatChannelId,
        ConversationStatusId = e.ConversationStatusId,
        StartedAt = e.StartedAt,
        LastInteractionAt = e.LastInteractionAt,
        ClosedAt = e.ClosedAt
    };
}
