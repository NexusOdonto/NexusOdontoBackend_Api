using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.ToothStatus;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class ToothStatusService(CatalogServiceCore<ToothStatus> core) : IToothStatusService
{
    public Task<IReadOnlyList<ToothStatusDto>> GetAllAsync() =>
        core.GetAllAsync(EnglishCatalogMapping.ToToothStatusDto);

    public Task<ToothStatusDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, EnglishCatalogMapping.ToToothStatusDto);

    public Task<ToothStatusDto> CreateAsync(CreateToothStatusRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new ToothStatus(request.Code, request.Name, request.Description, request.ColorHex, request.VisualCode, request.SortOrder);
        return core.CreateAsync(entity, EnglishCatalogMapping.ToToothStatusDto);
    }

    public Task<ToothStatusDto?> UpdateAsync(Guid id, UpdateToothStatusRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.ColorHex, request.VisualCode, request.SortOrder); EnglishCatalogMapping.ApplyActive(entity, request.IsActive);
        }, EnglishCatalogMapping.ToToothStatusDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
