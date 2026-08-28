using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.Catalogs;
using Application.Contracts.Repositories.Chatbot;
using Application.Contracts.Services.Chatbot;
using Application.DTOs.Chatbot.ChatbotConversation;
using Domain.Entities.Catalogs;
using Domain.Entities.Chatbot;

namespace Application.Services.Chatbot;

public sealed class ChatbotConversationService(
    IChatbotConversationRepository repository,
    ICatalogRepository<ChatbotConversationStatusEntity> statusCatalog,
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

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return false;
        await ServiceWrite.InTransactionAsync(unitOfWork, () => { repository.Remove(entity); return Task.CompletedTask; });
        return true;
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
