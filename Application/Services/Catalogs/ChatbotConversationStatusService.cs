using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.ChatbotConversationStatus;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class ChatbotConversationStatusService(CatalogServiceCore<ChatbotConversationStatusEntity> core) : IChatbotConversationStatusService
{
    public Task<IReadOnlyList<ChatbotConversationStatusDto>> GetAllAsync() =>
        core.GetAllAsync(CatalogMapping.ToChatbotConversationStatusDto);

    public Task<ChatbotConversationStatusDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, CatalogMapping.ToChatbotConversationStatusDto);

    public Task<ChatbotConversationStatusDto> CreateAsync(CreateChatbotConversationStatusRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new ChatbotConversationStatusEntity(request.Code, request.Name, request.Description, request.IsFinal, request.SortOrder);
        return core.CreateAsync(entity, CatalogMapping.ToChatbotConversationStatusDto);
    }

    public Task<ChatbotConversationStatusDto?> UpdateAsync(Guid id, UpdateChatbotConversationStatusRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.IsFinal, request.SortOrder); CatalogMapping.ApplyActive(entity, request.IsActive);
        }, CatalogMapping.ToChatbotConversationStatusDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
