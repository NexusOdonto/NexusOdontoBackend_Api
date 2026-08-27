using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogos;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class SexService(CatalogServiceCore<Sex> core) : ISexService
{
    public Task<IReadOnlyList<SexoDto>> GetAllAsync() =>
        core.GetAllAsync(SpanishCatalogMapping.ToSexoDto);

    public Task<SexoDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, SpanishCatalogMapping.ToSexoDto);

    public Task<SexoDto> CreateAsync(SexoDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new Sex(request.Codigo, request.Nombre, request.Descripcion, request.Orden);
        entity.IsActive = request.Activo;
        return core.CreateAsync(entity, SpanishCatalogMapping.ToSexoDto);
    }

    public Task<SexoDto?> UpdateAsync(Guid id, SexoDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Nombre, request.Descripcion, request.Orden); SpanishCatalogMapping.ApplyActive(entity, request.Activo);
        }, SpanishCatalogMapping.ToSexoDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
