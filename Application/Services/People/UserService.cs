using Application.Common;
using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.People;
using Application.Contracts.Services.People;
using Application.DTOs.Users;
using Application.Exceptions;
using Domain.Entities.People;
using Domain.Entities.Security;

namespace Application.Services.People;

public sealed class UserService(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IUserService
{
    public async Task<IReadOnlyList<UserDetailResponseDto>> GetAllAsync()
    {
        var items = await userRepository.GetAllWithRolesAsync();
        return items.Select(Map).ToList();
    }

    public async Task<UserDetailResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await userRepository.GetByIdWithRolesAsync(id)
            ?? await userRepository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<UserDetailResponseDto> CreateAsync(CreateUserDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var existingUser = await userRepository.GetByPersonIdAsync(request.PersonId);
        if (existingUser is not null)
        {
            throw new ConflictException("A user already exists for this person.");
        }

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var entity = new UserEntity(request.PersonId, passwordHash);

            var rolesToAdd = new HashSet<Guid>();
            if (request.RoleId.HasValue && request.RoleId.Value != Guid.Empty)
            {
                rolesToAdd.Add(request.RoleId.Value);
            }
            if (request.RoleIds is not null)
            {
                foreach (var rid in request.RoleIds.Where(id => id != Guid.Empty))
                {
                    rolesToAdd.Add(rid);
                }
            }

            foreach (var roleId in rolesToAdd)
            {
                entity.UserRoles.Add(new UserRoleEntity(entity.Id, roleId));
            }

            await userRepository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<UserDetailResponseDto?> UpdateAsync(Guid id, UpdateUserDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await userRepository.GetByIdAsync(id);
        if (entity is null) return null;

        if (SeedAdminGuard.IsProtectedUser(entity.Id) && !request.IsActive)
        {
            throw new BusinessRuleException(SeedAdminGuard.CannotDeactivateOrDeleteMessage);
        }

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.IsActive = request.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;
            userRepository.Update(entity);
            return Map(entity);
        });
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await userRepository.GetByIdAsync(id);
        if (entity is null) return false;

        if (SeedAdminGuard.IsProtectedUser(entity.Id))
        {
            throw new BusinessRuleException(SeedAdminGuard.CannotDeactivateOrDeleteMessage);
        }

        await ServiceWrite.InTransactionAsync(unitOfWork, () =>
        {
            entity.Deactivate();
            userRepository.Update(entity);
            return Task.CompletedTask;
        });
        return true;
    }

    private static UserDetailResponseDto Map(UserEntity e) => new()
    {
        Id = e.Id,
        PersonId = e.PersonId,
        LastAccessAt = e.LastAccessAt,
        RoleIds = e.UserRoles.Select(ur => ur.RoleId).ToList(),
        IsActive = e.IsActive,
        CreatedAt = e.CreatedAt
    };
}
