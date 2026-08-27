using Application.Contracts.Repositories;
using Application.Contracts.Services.Security;
using Application.DTOs.Permisos;
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

    public async Task<object> CreateAsync(CreatePermisoDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new Permission(request.Modulo, request.AccionPermisoId, request.Descripcion);
            await permissionRepository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<object?> UpdateAsync(Guid id, UpdatePermisoDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await permissionRepository.GetByIdAsync(id);
        if (entity is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.Update(request.Modulo, request.AccionPermisoId, request.Descripcion);
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

    private static object Map(Permission e) => new
    {
        e.Id,
        e.Module,
        e.ActionPermissionId,
        e.Description
    };
}
