using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogos;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class SpecialtyService(CatalogServiceCore<Specialty> core) : ISpecialtyService
{
    public Task<IReadOnlyList<EspecialidadDto>> GetAllAsync() =>
        core.GetAllAsync(SpanishCatalogMapping.ToEspecialidadDto);

    public Task<EspecialidadDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, SpanishCatalogMapping.ToEspecialidadDto);

    public Task<EspecialidadDto> CreateAsync(EspecialidadDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new Specialty(request.Codigo, request.Nombre, request.Descripcion, request.Orden);
        entity.IsActive = request.Activo;
        return core.CreateAsync(entity, SpanishCatalogMapping.ToEspecialidadDto);
    }

    public Task<EspecialidadDto?> UpdateAsync(Guid id, EspecialidadDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Nombre, request.Descripcion, request.Orden); SpanishCatalogMapping.ApplyActive(entity, request.Activo);
        }, SpanishCatalogMapping.ToEspecialidadDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
