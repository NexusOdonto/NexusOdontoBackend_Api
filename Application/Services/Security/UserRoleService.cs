using Application.Contracts.Repositories;
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
        var user = await userRepository.GetByIdWithRolesAsync(id);
        var role = user?.UserRoles.FirstOrDefault();
        return role is null
            ? null
            : new UsuarioRolDto { UsuarioId = role.UserId, RolId = role.RoleId };
    }

    public async Task<UsuarioRolDto> CreateAsync(UsuarioRolDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var user = await userRepository.GetByIdWithRolesAsync(request.UsuarioId)
            ?? throw new InvalidOperationException($"User {request.UsuarioId} was not found.");

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            if (user.UserRoles.All(ur => ur.RoleId != request.RolId))
            {
                user.UserRoles.Add(new UserRole(request.UsuarioId, request.RolId));
                userRepository.Update(user);
            }

            return new UsuarioRolDto { UsuarioId = request.UsuarioId, RolId = request.RolId };
        });
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await userRepository.GetByIdWithRolesAsync(id);
        if (user is null || user.UserRoles.Count == 0) return false;

        await ServiceWrite.InTransactionAsync(unitOfWork, () =>
        {
            user.UserRoles.Clear();
            userRepository.Update(user);
            return Task.CompletedTask;
        });
        return true;
    }
}
