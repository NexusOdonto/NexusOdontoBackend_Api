using Application.Contracts.Services.Catalogs;
using Application.DTOs.Catalogs.ActionPermission;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class ActionPermissionService(CatalogServiceCore<ActionPermissionEntity> core) : IActionPermissionService
{
    public Task<IReadOnlyList<ActionPermissionDto>> GetAllAsync() =>
        core.GetAllAsync(CatalogMapping.ToActionPermissionDto);

    public Task<ActionPermissionDto?> GetByIdAsync(Guid id) =>
        core.GetByIdAsync(id, CatalogMapping.ToActionPermissionDto);

    public Task<ActionPermissionDto> CreateAsync(ActionPermissionDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = new ActionPermissionEntity(request.Code, request.Name, request.Description, request.SortOrder);
        entity.IsActive = request.IsActive;
        return core.CreateAsync(entity, CatalogMapping.ToActionPermissionDto);
    }

    public Task<ActionPermissionDto?> UpdateAsync(Guid id, ActionPermissionDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return core.UpdateAsync(id, entity =>
        {
            entity.Update(request.Name, request.Description, request.SortOrder);
            CatalogMapping.ApplyActive(entity, request.IsActive);
        }, CatalogMapping.ToActionPermissionDto);
    }

    public Task<bool> DeleteAsync(Guid id) => core.DeleteAsync(id);
}
