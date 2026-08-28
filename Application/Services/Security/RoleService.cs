using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.Security;
using Application.Contracts.Services.Security;
using Application.DTOs.Roles;
using Domain.Entities.Security;

namespace Application.Services.Security;

public sealed class RoleService(
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork) : IRoleService
{
    public async Task<IReadOnlyList<object>> GetAllAsync()
    {
        var items = await roleRepository.GetAllAsync();
        return items.Select(Map).Cast<object>().ToList();
    }

    public async Task<object?> GetByIdAsync(Guid id)
    {
        var entity = await roleRepository.GetByIdWithPermissionsAsync(id)
            ?? await roleRepository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<object> CreateAsync(CreateRolDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new RoleEntity(request.Codigo, request.Nombre, request.Descripcion);
            await roleRepository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<object?> UpdateAsync(Guid id, UpdateRolDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await roleRepository.GetByIdAsync(id);
        if (entity is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.Code = request.Codigo;
            entity.Update(request.Nombre, request.Descripcion);
            entity.IsActive = request.Activo;
            roleRepository.Update(entity);
            return Map(entity);
        });
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await roleRepository.GetByIdAsync(id);
        if (entity is null) return false;

        await ServiceWrite.InTransactionAsync(unitOfWork, () =>
        {
            entity.Deactivate();
            roleRepository.Update(entity);
            return Task.CompletedTask;
        });
        return true;
    }

    private static object Map(RoleEntity e) => new
    {
        e.Id,
        e.Code,
        e.Name,
        e.Description,
        PermissionIds = e.Permissions.Select(p => p.Id).ToList(),
        e.IsActive,
        e.CreatedAt
    };
}
