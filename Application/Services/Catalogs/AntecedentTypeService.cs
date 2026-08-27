using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.AntecedentType;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class AntecedentTypeService(CatalogServiceCore<AntecedentType> core) : IAntecedentTypeService
{
    public Task<IReadOnlyList<AntecedentTypeDto>> GetAllAsync() =>
        core.GetAllAsync(EnglishCatalogMapping.ToAntecedentTypeDto);

    public Task<AntecedentTypeDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, EnglishCatalogMapping.ToAntecedentTypeDto);

    public Task<AntecedentTypeDto> CreateAsync(CreateAntecedentTypeRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new AntecedentType(request.Code, request.Name, request.Description, request.SortOrder);
        return core.CreateAsync(entity, EnglishCatalogMapping.ToAntecedentTypeDto);
    }

    public Task<AntecedentTypeDto?> UpdateAsync(Guid id, UpdateAntecedentTypeRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.SortOrder); EnglishCatalogMapping.ApplyActive(entity, request.IsActive);
        }, EnglishCatalogMapping.ToAntecedentTypeDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
