using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.DentitionType;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class DentitionTypeService(CatalogServiceCore<DentitionType> core) : IDentitionTypeService
{
    public Task<IReadOnlyList<DentitionTypeDto>> GetAllAsync() =>
        core.GetAllAsync(EnglishCatalogMapping.ToDentitionTypeDto);

    public Task<DentitionTypeDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, EnglishCatalogMapping.ToDentitionTypeDto);

    public Task<DentitionTypeDto> CreateAsync(CreateDentitionTypeRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new DentitionType(request.Code, request.Name, request.Description, request.SortOrder);
        return core.CreateAsync(entity, EnglishCatalogMapping.ToDentitionTypeDto);
    }

    public Task<DentitionTypeDto?> UpdateAsync(Guid id, UpdateDentitionTypeRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.SortOrder); EnglishCatalogMapping.ApplyActive(entity, request.IsActive);
        }, EnglishCatalogMapping.ToDentitionTypeDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
