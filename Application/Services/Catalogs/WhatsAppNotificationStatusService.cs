using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogos;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class WhatsAppNotificationStatusService(CatalogServiceCore<WhatsAppNotificationStatus> core) : IWhatsAppNotificationStatusService
{
    public Task<IReadOnlyList<EstadoNotificacionWhatsappDto>> GetAllAsync() =>
        core.GetAllAsync(SpanishCatalogMapping.ToEstadoNotificacionWhatsappDto);

    public Task<EstadoNotificacionWhatsappDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, SpanishCatalogMapping.ToEstadoNotificacionWhatsappDto);

    public Task<EstadoNotificacionWhatsappDto> CreateAsync(EstadoNotificacionWhatsappDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new WhatsAppNotificationStatus(request.Codigo, request.Nombre, request.Descripcion, request.Orden);
        entity.IsActive = request.Activo;
        return core.CreateAsync(entity, SpanishCatalogMapping.ToEstadoNotificacionWhatsappDto);
    }

    public Task<EstadoNotificacionWhatsappDto?> UpdateAsync(Guid id, EstadoNotificacionWhatsappDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Nombre, request.Descripcion, request.Orden); SpanishCatalogMapping.ApplyActive(entity, request.Activo);
        }, SpanishCatalogMapping.ToEstadoNotificacionWhatsappDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
