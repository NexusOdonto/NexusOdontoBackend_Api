using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.HubNotificationType;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class HubNotificationTypeService(CatalogServiceCore<HubNotificationTypeEntity> core) : IHubNotificationTypeService
{
    public Task<IReadOnlyList<HubNotificationTypeDto>> GetAllAsync() =>
        core.GetAllAsync(CatalogMapping.ToHubNotificationTypeDto);

    public Task<HubNotificationTypeDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, CatalogMapping.ToHubNotificationTypeDto);

    public Task<HubNotificationTypeDto> CreateAsync(HubNotificationTypeDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new HubNotificationTypeEntity(request.Code, request.Name, request.Description, request.Icon, request.SortOrder);
        entity.IsActive = request.IsActive;
        return core.CreateAsync(entity, CatalogMapping.ToHubNotificationTypeDto);
    }

    public Task<HubNotificationTypeDto?> UpdateAsync(Guid id, HubNotificationTypeDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.Icon, request.SortOrder);
            CatalogMapping.ApplyActive(entity, request.IsActive);
        }, CatalogMapping.ToHubNotificationTypeDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
