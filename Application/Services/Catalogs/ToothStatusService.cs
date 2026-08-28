using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.ToothStatus;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class ToothStatusService(CatalogServiceCore<ToothStatusEntity> core) : IToothStatusService
{
    public Task<IReadOnlyList<ToothStatusDto>> GetAllAsync() =>
        core.GetAllAsync(CatalogMapping.ToToothStatusDto);

    public Task<ToothStatusDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, CatalogMapping.ToToothStatusDto);

    public Task<ToothStatusDto> CreateAsync(CreateToothStatusRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new ToothStatusEntity(request.Code, request.Name, request.Description, request.ColorHex, request.VisualCode, request.SortOrder);
        return core.CreateAsync(entity, CatalogMapping.ToToothStatusDto);
    }

    public Task<ToothStatusDto?> UpdateAsync(Guid id, UpdateToothStatusRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.ColorHex, request.VisualCode, request.SortOrder); CatalogMapping.ApplyActive(entity, request.IsActive);
        }, CatalogMapping.ToToothStatusDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
