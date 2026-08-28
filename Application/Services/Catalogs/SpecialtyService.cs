using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.Specialty;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class SpecialtyService(CatalogServiceCore<SpecialtyEntity> core) : ISpecialtyService
{
    public Task<IReadOnlyList<SpecialtyDto>> GetAllAsync() =>
        core.GetAllAsync(CatalogMapping.ToSpecialtyDto);

    public Task<SpecialtyDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, CatalogMapping.ToSpecialtyDto);

    public Task<SpecialtyDto> CreateAsync(SpecialtyDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new SpecialtyEntity(request.Code, request.Name, request.Description, request.SortOrder);
        entity.IsActive = request.IsActive;
        return core.CreateAsync(entity, CatalogMapping.ToSpecialtyDto);
    }

    public Task<SpecialtyDto?> UpdateAsync(Guid id, SpecialtyDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.SortOrder);
            CatalogMapping.ApplyActive(entity, request.IsActive);
        }, CatalogMapping.ToSpecialtyDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
