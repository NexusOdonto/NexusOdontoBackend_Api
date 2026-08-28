using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.Sex;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class SexService(CatalogServiceCore<SexEntity> core) : ISexService
{
    public Task<IReadOnlyList<SexDto>> GetAllAsync() =>
        core.GetAllAsync(CatalogMapping.ToSexDto);

    public Task<SexDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, CatalogMapping.ToSexDto);

    public Task<SexDto> CreateAsync(SexDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new SexEntity(request.Code, request.Name, request.Description, request.SortOrder);
        entity.IsActive = request.IsActive;
        return core.CreateAsync(entity, CatalogMapping.ToSexDto);
    }

    public Task<SexDto?> UpdateAsync(Guid id, SexDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.SortOrder);
            CatalogMapping.ApplyActive(entity, request.IsActive);
        }, CatalogMapping.ToSexDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
