using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.People;
using Application.Contracts.Services.People;
using Application.DTOs.Users;
using Domain.Entities.People;

namespace Application.Services.People;

public sealed class UserService(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IUserService
{
    public async Task<IReadOnlyList<object>> GetAllAsync()
    {
        var items = await userRepository.GetAllWithRolesAsync();
        return items.Select(Map).Cast<object>().ToList();
    }

    public async Task<object?> GetByIdAsync(Guid id)
    {
        var entity = await userRepository.GetByIdWithRolesAsync(id)
            ?? await userRepository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<object> CreateAsync(CreateUserDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var entity = new UserEntity(request.PersonId, passwordHash);
            await userRepository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<object?> UpdateAsync(Guid id, UpdateUserDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await userRepository.GetByIdAsync(id);
        if (entity is null) return null;

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

        await ServiceWrite.InTransactionAsync(unitOfWork, () =>
        {
            entity.Deactivate();
            userRepository.Update(entity);
            return Task.CompletedTask;
        });
        return true;
    }

    private static object Map(UserEntity e) => new
    {
        e.Id,
        e.PersonId,
        e.LastAccessAt,
        RoleIds = e.UserRoles.Select(ur => ur.RoleId).ToList(),
        e.IsActive,
        e.CreatedAt
    };
}
