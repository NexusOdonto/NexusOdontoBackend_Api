using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.WhatsAppNotificationStatus;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class WhatsAppNotificationStatusService(CatalogServiceCore<WhatsAppNotificationStatusEntity> core) : IWhatsAppNotificationStatusService
{
    public Task<IReadOnlyList<WhatsAppNotificationStatusDto>> GetAllAsync() =>
        core.GetAllAsync(CatalogMapping.ToWhatsAppNotificationStatusDto);

    public Task<WhatsAppNotificationStatusDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, CatalogMapping.ToWhatsAppNotificationStatusDto);

    public Task<WhatsAppNotificationStatusDto> CreateAsync(WhatsAppNotificationStatusDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new WhatsAppNotificationStatusEntity(request.Code, request.Name, request.Description, request.SortOrder);
        entity.IsActive = request.IsActive;
        return core.CreateAsync(entity, CatalogMapping.ToWhatsAppNotificationStatusDto);
    }

    public Task<WhatsAppNotificationStatusDto?> UpdateAsync(Guid id, WhatsAppNotificationStatusDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.SortOrder);
            CatalogMapping.ApplyActive(entity, request.IsActive);
        }, CatalogMapping.ToWhatsAppNotificationStatusDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
