using Application.Contracts.Repositories;
using Application.Contracts.Services.Chatbot;
using Application.DTOs.Chatbot.ChatbotConversation;
using Domain.Entities.Catalogs;
using Domain.Entities.Chatbot;

namespace Application.Services.Chatbot;

public sealed class ChatbotConversationService(
    IChatbotConversationRepository repository,
    ICatalogRepository<ChatbotConversationStatus> statusCatalog,
    IUnitOfWork unitOfWork) : IChatbotConversationService
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

    public async Task<object> CreateAsync(CreateChatbotConversationRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var statusId = await ServiceWrite.ResolveCatalogIdAsync(statusCatalog, "OPEN", "ACTIVE", "NEW");

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new ChatbotConversation(
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

    private static object Map(ChatbotConversation e) => new
    {
        e.Id, e.PatientId, e.ChatIdentifier, e.ChatChannelId, e.ConversationStatusId,
        e.StartedAt, e.LastInteractionAt, e.ClosedAt
    };
}
