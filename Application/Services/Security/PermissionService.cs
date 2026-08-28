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
    public async Task<IReadOnlyList<PermissionResponseDto>> GetAllAsync()
    {
        var items = await permissionRepository.GetAllAsync();
        return items.Select(Map).ToList();
    }

    public async Task<PermissionResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await permissionRepository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<PermissionResponseDto> CreateAsync(CreatePermissionDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new PermissionEntity(request.Module, request.ActionPermissionId, request.Description);
            await permissionRepository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<PermissionResponseDto?> UpdateAsync(Guid id, UpdatePermissionDto request)
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

    private static PermissionResponseDto Map(PermissionEntity e) => new()
    {
        Id = e.Id,
        Module = e.Module,
        ActionPermissionId = e.ActionPermissionId,
        Description = e.Description
    };
}
