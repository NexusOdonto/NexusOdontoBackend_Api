using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogos;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class HubNotificationStatusService(CatalogServiceCore<HubNotificationStatusEntity> core) : IHubNotificationStatusService
{
    public Task<IReadOnlyList<EstadoNotificacionHubDto>> GetAllAsync() =>
        core.GetAllAsync(SpanishCatalogMapping.ToEstadoNotificacionHubDto);

    public Task<EstadoNotificacionHubDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, SpanishCatalogMapping.ToEstadoNotificacionHubDto);

    public Task<EstadoNotificacionHubDto> CreateAsync(EstadoNotificacionHubDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new HubNotificationStatusEntity(request.Codigo, request.Nombre, request.Descripcion, request.Orden);
        entity.IsActive = request.Activo;
        return core.CreateAsync(entity, SpanishCatalogMapping.ToEstadoNotificacionHubDto);
    }

    public Task<EstadoNotificacionHubDto?> UpdateAsync(Guid id, EstadoNotificacionHubDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Nombre, request.Descripcion, request.Orden); SpanishCatalogMapping.ApplyActive(entity, request.Activo);
        }, SpanishCatalogMapping.ToEstadoNotificacionHubDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
