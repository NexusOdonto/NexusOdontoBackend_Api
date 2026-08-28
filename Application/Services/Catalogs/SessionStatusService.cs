using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogos;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class SessionStatusService(CatalogServiceCore<SessionStatusEntity> core) : ISessionStatusService
{
    public Task<IReadOnlyList<EstadoSesionDto>> GetAllAsync() =>
        core.GetAllAsync(SpanishCatalogMapping.ToEstadoSesionDto);

    public Task<EstadoSesionDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, SpanishCatalogMapping.ToEstadoSesionDto);

    public Task<EstadoSesionDto> CreateAsync(EstadoSesionDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new SessionStatusEntity(request.Codigo, request.Nombre, request.Descripcion, request.Orden);
        entity.IsActive = request.Activo;
        return core.CreateAsync(entity, SpanishCatalogMapping.ToEstadoSesionDto);
    }

    public Task<EstadoSesionDto?> UpdateAsync(Guid id, EstadoSesionDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Nombre, request.Descripcion, request.Orden); SpanishCatalogMapping.ApplyActive(entity, request.Activo);
        }, SpanishCatalogMapping.ToEstadoSesionDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
