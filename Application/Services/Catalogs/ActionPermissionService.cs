using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogos;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class ActionPermissionService(CatalogServiceCore<ActionPermission> core) : IActionPermissionService
{
    public Task<IReadOnlyList<AccionPermisoDto>> GetAllAsync() =>
        core.GetAllAsync(SpanishCatalogMapping.ToAccionPermisoDto);

    public Task<AccionPermisoDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, SpanishCatalogMapping.ToAccionPermisoDto);

    public Task<AccionPermisoDto> CreateAsync(AccionPermisoDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new ActionPermission(request.Codigo, request.Nombre, request.Descripcion, request.Orden);
        entity.IsActive = request.Activo;
        return core.CreateAsync(entity, SpanishCatalogMapping.ToAccionPermisoDto);
    }

    public Task<AccionPermisoDto?> UpdateAsync(Guid id, AccionPermisoDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Nombre, request.Descripcion, request.Orden); SpanishCatalogMapping.ApplyActive(entity, request.Activo);
        }, SpanishCatalogMapping.ToAccionPermisoDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
