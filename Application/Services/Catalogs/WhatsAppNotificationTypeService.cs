using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogos;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class WhatsAppNotificationTypeService(CatalogServiceCore<WhatsAppNotificationTypeEntity> core) : IWhatsAppNotificationTypeService
{
    public Task<IReadOnlyList<TipoNotificacionWhatsappDto>> GetAllAsync() =>
        core.GetAllAsync(SpanishCatalogMapping.ToTipoNotificacionWhatsappDto);

    public Task<TipoNotificacionWhatsappDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, SpanishCatalogMapping.ToTipoNotificacionWhatsappDto);

    public Task<TipoNotificacionWhatsappDto> CreateAsync(TipoNotificacionWhatsappDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new WhatsAppNotificationTypeEntity(request.Codigo, request.Nombre, request.Descripcion, request.PlantillaPredeterminada, request.Orden);
        entity.IsActive = request.Activo;
        return core.CreateAsync(entity, SpanishCatalogMapping.ToTipoNotificacionWhatsappDto);
    }

    public Task<TipoNotificacionWhatsappDto?> UpdateAsync(Guid id, TipoNotificacionWhatsappDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Nombre, request.Descripcion, request.PlantillaPredeterminada, request.Orden); SpanishCatalogMapping.ApplyActive(entity, request.Activo);
        }, SpanishCatalogMapping.ToTipoNotificacionWhatsappDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
