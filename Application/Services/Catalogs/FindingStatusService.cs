using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.FindingStatus;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class FindingStatusService(CatalogServiceCore<FindingStatusEntity> core) : IFindingStatusService
{
    public Task<IReadOnlyList<FindingStatusDto>> GetAllAsync() =>
        core.GetAllAsync(EnglishCatalogMapping.ToFindingStatusDto);

    public Task<FindingStatusDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, EnglishCatalogMapping.ToFindingStatusDto);

    public Task<FindingStatusDto> CreateAsync(CreateFindingStatusRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new FindingStatusEntity(request.Code, request.Name, request.Description, request.ColorHex, request.SortOrder);
        return core.CreateAsync(entity, EnglishCatalogMapping.ToFindingStatusDto);
    }

    public Task<FindingStatusDto?> UpdateAsync(Guid id, UpdateFindingStatusRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.ColorHex, request.SortOrder); EnglishCatalogMapping.ApplyActive(entity, request.IsActive);
        }, EnglishCatalogMapping.ToFindingStatusDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
