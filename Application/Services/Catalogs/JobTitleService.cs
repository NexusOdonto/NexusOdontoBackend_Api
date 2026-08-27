using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogos;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class JobTitleService(CatalogServiceCore<JobTitle> core) : IJobTitleService
{
    public Task<IReadOnlyList<CargoDto>> GetAllAsync() =>
        core.GetAllAsync(SpanishCatalogMapping.ToCargoDto);

    public Task<CargoDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, SpanishCatalogMapping.ToCargoDto);

    public Task<CargoDto> CreateAsync(CargoDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new JobTitle(request.Codigo, request.Nombre, request.Descripcion, request.Orden);
        entity.IsActive = request.Activo;
        return core.CreateAsync(entity, SpanishCatalogMapping.ToCargoDto);
    }

    public Task<CargoDto?> UpdateAsync(Guid id, CargoDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Nombre, request.Descripcion, request.Orden); SpanishCatalogMapping.ApplyActive(entity, request.Activo);
        }, SpanishCatalogMapping.ToCargoDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
