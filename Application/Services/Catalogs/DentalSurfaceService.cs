using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.DentalSurface;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class DentalSurfaceService(CatalogServiceCore<DentalSurfaceEntity> core) : IDentalSurfaceService
{
    public Task<IReadOnlyList<DentalSurfaceDto>> GetAllAsync() =>
        core.GetAllAsync(CatalogMapping.ToDentalSurfaceDto);

    public Task<DentalSurfaceDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, CatalogMapping.ToDentalSurfaceDto);

    public Task<DentalSurfaceDto> CreateAsync(CreateDentalSurfaceRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new DentalSurfaceEntity(request.Code, request.Name, request.Description, request.AppliesToAnterior, request.AppliesToPosterior, request.SortOrder);
        return core.CreateAsync(entity, CatalogMapping.ToDentalSurfaceDto);
    }

    public Task<DentalSurfaceDto?> UpdateAsync(Guid id, UpdateDentalSurfaceRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.AppliesToAnterior, request.AppliesToPosterior, request.SortOrder); CatalogMapping.ApplyActive(entity, request.IsActive);
        }, CatalogMapping.ToDentalSurfaceDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
