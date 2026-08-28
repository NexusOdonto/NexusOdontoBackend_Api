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
    public async Task<IReadOnlyList<RoleResponseDto>> GetAllAsync()
    {
        var items = await roleRepository.GetAllAsync();
        return items.Select(Map).ToList();
    }

    public async Task<RoleResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await roleRepository.GetByIdWithPermissionsAsync(id)
            ?? await roleRepository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<RoleResponseDto> CreateAsync(CreateRoleDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new RoleEntity(request.Code, request.Name, request.Description);
            await roleRepository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<RoleResponseDto?> UpdateAsync(Guid id, UpdateRoleDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await roleRepository.GetByIdAsync(id);
        if (entity is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.Code = request.Code;
            entity.Update(request.Name, request.Description);
            entity.IsActive = request.IsActive;
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

    private static RoleResponseDto Map(RoleEntity e) => new()
    {
        Id = e.Id,
        Code = e.Code,
        Name = e.Name,
        Description = e.Description,
        PermissionIds = e.Permissions.Select(p => p.Id).ToList(),
        IsActive = e.IsActive,
        CreatedAt = e.CreatedAt
    };
}
