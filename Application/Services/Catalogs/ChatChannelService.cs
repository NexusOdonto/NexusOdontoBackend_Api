using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.ChatChannel;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class ChatChannelService(CatalogServiceCore<ChatChannelEntity> core) : IChatChannelService
{
    public Task<IReadOnlyList<ChatChannelDto>> GetAllAsync() =>
        core.GetAllAsync(CatalogMapping.ToChatChannelDto);

    public Task<ChatChannelDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, CatalogMapping.ToChatChannelDto);

    public Task<ChatChannelDto> CreateAsync(CreateChatChannelRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new ChatChannelEntity(request.Code, request.Name, request.Description, request.SortOrder);
        return core.CreateAsync(entity, CatalogMapping.ToChatChannelDto);
    }

    public Task<ChatChannelDto?> UpdateAsync(Guid id, UpdateChatChannelRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.SortOrder); CatalogMapping.ApplyActive(entity, request.IsActive);
        }, CatalogMapping.ToChatChannelDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
