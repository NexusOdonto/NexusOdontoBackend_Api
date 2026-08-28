using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.WhatsAppNotificationType;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class WhatsAppNotificationTypeService(CatalogServiceCore<WhatsAppNotificationTypeEntity> core) : IWhatsAppNotificationTypeService
{
    public Task<IReadOnlyList<WhatsAppNotificationTypeDto>> GetAllAsync() =>
        core.GetAllAsync(CatalogMapping.ToWhatsAppNotificationTypeDto);

    public Task<WhatsAppNotificationTypeDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, CatalogMapping.ToWhatsAppNotificationTypeDto);

    public Task<WhatsAppNotificationTypeDto> CreateAsync(WhatsAppNotificationTypeDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new WhatsAppNotificationTypeEntity(request.Code, request.Name, request.Description, request.DefaultTemplate, request.SortOrder);
        entity.IsActive = request.IsActive;
        return core.CreateAsync(entity, CatalogMapping.ToWhatsAppNotificationTypeDto);
    }

    public Task<WhatsAppNotificationTypeDto?> UpdateAsync(Guid id, WhatsAppNotificationTypeDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.DefaultTemplate, request.SortOrder);
            CatalogMapping.ApplyActive(entity, request.IsActive);
        }, CatalogMapping.ToWhatsAppNotificationTypeDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
