using Application.Contracts.Repositories.People;
using Domain.Entities.People;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.People;

public sealed class UserRepository : Common.Repository<UserEntity>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<UserEntity?> GetByEmailWithRolesAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();

        return await WithRoles()
            .FirstOrDefaultAsync(
                u => u.PersonEntity != null && u.PersonEntity.Email == normalizedEmail,
                cancellationToken);
    }

    public async Task<string?> GetEmailByDocumentNumberAsync(
        string documentNumber,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<PersonEntity>()
            .Where(p => p.DocumentNumber == documentNumber)
            .Select(p => p.Email)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<UserEntity?> GetByPersonIdAsync(
        Guid personId,
        CancellationToken cancellationToken = default)
    {
        return await Set.FirstOrDefaultAsync(u => u.PersonId == personId, cancellationToken);
    }

    public async Task<IReadOnlyList<UserEntity>> GetAllWithRolesAsync(
        CancellationToken cancellationToken = default)
    {
        return await WithRoles().ToListAsync(cancellationToken);
    }

    public async Task<UserEntity?> GetByIdWithRolesAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await WithRoles()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    private IQueryable<UserEntity> WithRoles()
    {
        return Set
            .Include(u => u.PersonEntity)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.RoleEntity)
                    .ThenInclude(r => r!.Permissions)
                        .ThenInclude(p => p.ActionPermissionEntity);
    }
}
