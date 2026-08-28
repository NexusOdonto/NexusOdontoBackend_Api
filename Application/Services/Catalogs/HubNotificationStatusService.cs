using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.HubNotificationStatus;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class HubNotificationStatusService(CatalogServiceCore<HubNotificationStatusEntity> core) : IHubNotificationStatusService
{
    public Task<IReadOnlyList<HubNotificationStatusDto>> GetAllAsync() =>
        core.GetAllAsync(CatalogMapping.ToHubNotificationStatusDto);

    public Task<HubNotificationStatusDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, CatalogMapping.ToHubNotificationStatusDto);

    public Task<HubNotificationStatusDto> CreateAsync(HubNotificationStatusDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new HubNotificationStatusEntity(request.Code, request.Name, request.Description, request.SortOrder);
        entity.IsActive = request.IsActive;
        return core.CreateAsync(entity, CatalogMapping.ToHubNotificationStatusDto);
    }

    public Task<HubNotificationStatusDto?> UpdateAsync(Guid id, HubNotificationStatusDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.SortOrder);
            CatalogMapping.ApplyActive(entity, request.IsActive);
        }, CatalogMapping.ToHubNotificationStatusDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
