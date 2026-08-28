using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.Security;
using Application.Contracts.Services.Security;
using Application.DTOs.Permissions;
using Domain.Entities.Security;

namespace Application.Services.Security;

public sealed class PermissionService(
    IPermissionRepository permissionRepository,
    IUnitOfWork unitOfWork) : IPermissionService
{
    public async Task<IReadOnlyList<object>> GetAllAsync()
    {
        var items = await permissionRepository.GetAllAsync();
        return items.Select(Map).Cast<object>().ToList();
    }

    public async Task<object?> GetByIdAsync(Guid id)
    {
        var entity = await permissionRepository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<object> CreateAsync(CreatePermissionDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new PermissionEntity(request.Module, request.ActionPermissionId, request.Description);
            await permissionRepository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<object?> UpdateAsync(Guid id, UpdatePermissionDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await permissionRepository.GetByIdAsync(id);
        if (entity is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.Update(request.Module, request.ActionPermissionId, request.Description);
            permissionRepository.Update(entity);
            return Map(entity);
        });
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await permissionRepository.GetByIdAsync(id);
        if (entity is null) return false;

        await ServiceWrite.InTransactionAsync(unitOfWork, () =>
        {
            permissionRepository.Remove(entity);
            return Task.CompletedTask;
        });
        return true;
    }

    private static object Map(PermissionEntity e) => new
    {
        e.Id,
        e.Module,
        e.ActionPermissionId,
        e.Description
    };
}
