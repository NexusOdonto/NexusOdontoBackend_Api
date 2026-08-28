using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogos;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class HubNotificationTypeService(CatalogServiceCore<HubNotificationTypeEntity> core) : IHubNotificationTypeService
{
    public Task<IReadOnlyList<TipoNotificacionHubDto>> GetAllAsync() =>
        core.GetAllAsync(SpanishCatalogMapping.ToTipoNotificacionHubDto);

    public Task<TipoNotificacionHubDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, SpanishCatalogMapping.ToTipoNotificacionHubDto);

    public Task<TipoNotificacionHubDto> CreateAsync(TipoNotificacionHubDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new HubNotificationTypeEntity(request.Codigo, request.Nombre, request.Descripcion, request.Icono, request.Orden);
        entity.IsActive = request.Activo;
        return core.CreateAsync(entity, SpanishCatalogMapping.ToTipoNotificacionHubDto);
    }

    public Task<TipoNotificacionHubDto?> UpdateAsync(Guid id, TipoNotificacionHubDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Nombre, request.Descripcion, request.Icono, request.Orden); SpanishCatalogMapping.ApplyActive(entity, request.Activo);
        }, SpanishCatalogMapping.ToTipoNotificacionHubDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
