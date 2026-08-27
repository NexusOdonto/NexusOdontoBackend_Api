using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.DentalSurface;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class DentalSurfaceService(CatalogServiceCore<DentalSurface> core) : IDentalSurfaceService
{
    public Task<IReadOnlyList<DentalSurfaceDto>> GetAllAsync() =>
        core.GetAllAsync(EnglishCatalogMapping.ToDentalSurfaceDto);

    public Task<DentalSurfaceDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, EnglishCatalogMapping.ToDentalSurfaceDto);

    public Task<DentalSurfaceDto> CreateAsync(CreateDentalSurfaceRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new DentalSurface(request.Code, request.Name, request.Description, request.AppliesToAnterior, request.AppliesToPosterior, request.SortOrder);
        return core.CreateAsync(entity, EnglishCatalogMapping.ToDentalSurfaceDto);
    }

    public Task<DentalSurfaceDto?> UpdateAsync(Guid id, UpdateDentalSurfaceRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.AppliesToAnterior, request.AppliesToPosterior, request.SortOrder); EnglishCatalogMapping.ApplyActive(entity, request.IsActive);
        }, EnglishCatalogMapping.ToDentalSurfaceDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
