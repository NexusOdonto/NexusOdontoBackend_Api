using Application.Contracts.Repositories;
using Application.Contracts.Services.Chatbot;
using Application.DTOs.Chatbot.ChatbotMessage;
using Domain.Entities.Chatbot;

namespace Application.Services.Chatbot;

public sealed class ChatbotMessageService(
    IRepository<ChatbotMessage> repository,
    IUnitOfWork unitOfWork) : IChatbotMessageService
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

    public async Task<object> CreateAsync(CreateChatbotMessageRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new ChatbotMessage(
                request.ChatbotConversationId,
                request.MessageRoleId,
                request.Content,
                request.RagConfidence);
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

    private static object Map(ChatbotMessage e) => new
    {
        e.Id, e.ChatbotConversationId, e.MessageRoleId, e.Content, e.RagConfidence, e.CreatedAt
    };
}
