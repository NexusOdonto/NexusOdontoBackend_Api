using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.ChatbotMessageRole;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class ChatbotMessageRoleService(CatalogServiceCore<ChatbotMessageRoleEntity> core) : IChatbotMessageRoleService
{
    public Task<IReadOnlyList<ChatbotMessageRoleDto>> GetAllAsync() =>
        core.GetAllAsync(EnglishCatalogMapping.ToChatbotMessageRoleDto);

    public Task<ChatbotMessageRoleDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, EnglishCatalogMapping.ToChatbotMessageRoleDto);

    public Task<ChatbotMessageRoleDto> CreateAsync(CreateChatbotMessageRoleRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new ChatbotMessageRoleEntity(request.Code, request.Name, request.Description, request.SortOrder);
        return core.CreateAsync(entity, EnglishCatalogMapping.ToChatbotMessageRoleDto);
    }

    public Task<ChatbotMessageRoleDto?> UpdateAsync(Guid id, UpdateChatbotMessageRoleRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.SortOrder); EnglishCatalogMapping.ApplyActive(entity, request.IsActive);
        }, EnglishCatalogMapping.ToChatbotMessageRoleDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
