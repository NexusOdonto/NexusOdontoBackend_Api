using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.Chatbot;
using Application.Contracts.Services.Chatbot;
using Application.DTOs.Chatbot.ChatbotMessage;
using Domain.Entities.Chatbot;

namespace Application.Services.Chatbot;

public sealed class ChatbotMessageService(
    IRepository<ChatbotMessageEntity> repository,
    IChatbotConversationRepository conversationRepository,
    IUnitOfWork unitOfWork) : IChatbotMessageService
{
    public async Task<IReadOnlyList<ChatbotMessageResponseDto>> GetAllAsync()
    {
        var items = await repository.GetAllAsync();
        return items.Select(Map).ToList();
    }

    public async Task<ChatbotMessageResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<ChatbotMessageResponseDto> CreateAsync(CreateChatbotMessageRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new ChatbotMessageEntity(
                request.ChatbotConversationId,
                request.MessageRoleId,
                request.Content,
                request.RagConfidence);
            await repository.AddAsync(entity);

            // Actualizar la última interacción y reabrir conversación si estaba cerrada
            var conv = await conversationRepository.GetByIdAsync(request.ChatbotConversationId);
            if (conv is not null)
            {
                conv.LastInteractionAt = DateTime.UtcNow;
                if (conv.ClosedAt is not null)
                {
                    conv.ClosedAt = null;
                }
                conversationRepository.Update(conv);
            }

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

    private static ChatbotMessageResponseDto Map(ChatbotMessageEntity e) => new()
    {
        Id = e.Id,
        ChatbotConversationId = e.ChatbotConversationId,
        MessageRoleId = e.MessageRoleId,
        Content = e.Content,
        RagConfidence = e.RagConfidence,
        CreatedAt = e.CreatedAt
    };
}
