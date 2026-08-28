using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.People;
using Application.Contracts.Services.Security;
using Application.DTOs.UsuarioRoles;
using Domain.Entities.Security;

namespace Application.Services.Security;

public sealed class UserRoleService(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IUserRoleService
{
    public async Task<IReadOnlyList<UsuarioRolDto>> GetAllAsync()
    {
        var users = await userRepository.GetAllWithRolesAsync();
        return users
            .SelectMany(u => u.UserRoles.Select(ur => new UsuarioRolDto
            {
                UsuarioId = ur.UserId,
                RolId = ur.RoleId
            }))
            .ToList();
    }

    public async Task<UsuarioRolDto?> GetByIdAsync(Guid id)
    {
        var UserEntity = await userRepository.GetByIdWithRolesAsync(id);
        var RoleEntity = UserEntity?.UserRoles.FirstOrDefault();
        return RoleEntity is null
            ? null
            : new UsuarioRolDto { UsuarioId = RoleEntity.UserId, RolId = RoleEntity.RoleId };
    }

    public async Task<UsuarioRolDto> CreateAsync(UsuarioRolDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var UserEntity = await userRepository.GetByIdWithRolesAsync(request.UsuarioId)
            ?? throw new InvalidOperationException($"UserEntity {request.UsuarioId} was not found.");

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            if (UserEntity.UserRoles.All(ur => ur.RoleId != request.RolId))
            {
                UserEntity.UserRoles.Add(new UserRoleEntity(request.UsuarioId, request.RolId));
                userRepository.Update(UserEntity);
            }

            return new UsuarioRolDto { UsuarioId = request.UsuarioId, RolId = request.RolId };
        });
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var UserEntity = await userRepository.GetByIdWithRolesAsync(id);
        if (UserEntity is null || UserEntity.UserRoles.Count == 0) return false;

        await ServiceWrite.InTransactionAsync(unitOfWork, () =>
        {
            UserEntity.UserRoles.Clear();
            userRepository.Update(UserEntity);
            return Task.CompletedTask;
        });
        return true;
    }
}
