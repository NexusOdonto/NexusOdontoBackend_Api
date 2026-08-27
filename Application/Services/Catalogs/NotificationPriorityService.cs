using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogos;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class NotificationPriorityService(CatalogServiceCore<NotificationPriority> core) : INotificationPriorityService
{
    public Task<IReadOnlyList<PrioridadNotificacionDto>> GetAllAsync() =>
        core.GetAllAsync(SpanishCatalogMapping.ToPrioridadNotificacionDto);

    public Task<PrioridadNotificacionDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, SpanishCatalogMapping.ToPrioridadNotificacionDto);

    public Task<PrioridadNotificacionDto> CreateAsync(PrioridadNotificacionDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new NotificationPriority(request.Codigo, request.Nombre, request.Descripcion, request.ColorHex, request.Nivel, request.Orden);
        entity.IsActive = request.Activo;
        return core.CreateAsync(entity, SpanishCatalogMapping.ToPrioridadNotificacionDto);
    }

    public Task<PrioridadNotificacionDto?> UpdateAsync(Guid id, PrioridadNotificacionDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Nombre, request.Descripcion, request.ColorHex, request.Nivel, request.Orden); SpanishCatalogMapping.ApplyActive(entity, request.Activo);
        }, SpanishCatalogMapping.ToPrioridadNotificacionDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
