using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.People;
using Application.Contracts.Services.Security;
using Application.DTOs.UserRoles;
using Application.Exceptions;
using Domain.Entities.Security;

namespace Application.Services.Security;

public sealed class UserRoleService(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IUserRoleService
{
    public async Task<IReadOnlyList<UserRoleDto>> GetAllAsync()
    {
        var users = await userRepository.GetAllWithRolesAsync();
        return users
            .SelectMany(u => u.UserRoles.Select(ur => new UserRoleDto
            {
                UserId = ur.UserId,
                RoleId = ur.RoleId
            }))
            .ToList();
    }

    public async Task<UserRoleDto?> GetByIdAsync(Guid id)
    {
        var user = await userRepository.GetByIdWithRolesAsync(id);
        var userRole = user?.UserRoles.FirstOrDefault();
        return userRole is null
            ? null
            : new UserRoleDto { UserId = userRole.UserId, RoleId = userRole.RoleId };
    }

    public async Task<UserRoleDto> CreateAsync(UserRoleDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var user = await userRepository.GetByIdWithRolesAsync(request.UserId)
            ?? throw new BusinessRuleException($"User {request.UserId} was not found.");

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            if (user.UserRoles.All(ur => ur.RoleId != request.RoleId))
            {
                user.UserRoles.Add(new UserRoleEntity(request.UserId, request.RoleId));
                userRepository.Update(user);
            }

            return new UserRoleDto { UserId = request.UserId, RoleId = request.RoleId };
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
