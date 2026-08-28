using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.FindingType;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class FindingTypeService(CatalogServiceCore<FindingTypeEntity> core) : IFindingTypeService
{
    public Task<IReadOnlyList<FindingTypeDto>> GetAllAsync() =>
        core.GetAllAsync(CatalogMapping.ToFindingTypeDto);

    public Task<FindingTypeDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, CatalogMapping.ToFindingTypeDto);

    public Task<FindingTypeDto> CreateAsync(CreateFindingTypeRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new FindingTypeEntity(request.Code, request.Name, request.Description, request.ColorHex, request.VisualSymbol, request.RequiresSurface, request.SortOrder);
        return core.CreateAsync(entity, CatalogMapping.ToFindingTypeDto);
    }

    public Task<FindingTypeDto?> UpdateAsync(Guid id, UpdateFindingTypeRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.ColorHex, request.VisualSymbol, request.RequiresSurface, request.SortOrder); CatalogMapping.ApplyActive(entity, request.IsActive);
        }, CatalogMapping.ToFindingTypeDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
