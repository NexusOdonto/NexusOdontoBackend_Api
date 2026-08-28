using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.NotificationPriority;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class NotificationPriorityService(CatalogServiceCore<NotificationPriorityEntity> core) : INotificationPriorityService
{
    public Task<IReadOnlyList<NotificationPriorityDto>> GetAllAsync() =>
        core.GetAllAsync(CatalogMapping.ToNotificationPriorityDto);

    public Task<NotificationPriorityDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, CatalogMapping.ToNotificationPriorityDto);

    public Task<NotificationPriorityDto> CreateAsync(NotificationPriorityDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new NotificationPriorityEntity(request.Code, request.Name, request.Description, request.ColorHex, request.Level, request.SortOrder);
        entity.IsActive = request.IsActive;
        return core.CreateAsync(entity, CatalogMapping.ToNotificationPriorityDto);
    }

    public Task<NotificationPriorityDto?> UpdateAsync(Guid id, NotificationPriorityDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.ColorHex, request.Level, request.SortOrder);
            CatalogMapping.ApplyActive(entity, request.IsActive);
        }, CatalogMapping.ToNotificationPriorityDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
