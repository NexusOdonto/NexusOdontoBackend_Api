using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.AntecedentType;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class AntecedentTypeService(CatalogServiceCore<AntecedentTypeEntity> core) : IAntecedentTypeService
{
    public Task<IReadOnlyList<AntecedentTypeDto>> GetAllAsync() =>
        core.GetAllAsync(CatalogMapping.ToAntecedentTypeDto);

    public Task<AntecedentTypeDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, CatalogMapping.ToAntecedentTypeDto);

    public Task<AntecedentTypeDto> CreateAsync(CreateAntecedentTypeRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new AntecedentTypeEntity(request.Code, request.Name, request.Description, request.SortOrder);
        return core.CreateAsync(entity, CatalogMapping.ToAntecedentTypeDto);
    }

    public Task<AntecedentTypeDto?> UpdateAsync(Guid id, UpdateAntecedentTypeRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.SortOrder); CatalogMapping.ApplyActive(entity, request.IsActive);
        }, CatalogMapping.ToAntecedentTypeDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
