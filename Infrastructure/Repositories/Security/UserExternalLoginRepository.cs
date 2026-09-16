using Application.Contracts.Repositories.Security;
using Domain.Entities.Security;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Security;

public sealed class UserExternalLoginRepository(AppDbContext context) : IUserExternalLoginRepository
{
    public async Task<UserExternalLoginEntity?> GetByProviderKeyAsync(
        string provider,
        string providerKey,
        CancellationToken cancellationToken = default)
    {
        return await context.Set<UserExternalLoginEntity>()
            .FirstOrDefaultAsync(
                e => e.Provider == provider && e.ProviderKey == providerKey,
                cancellationToken);
    }

    public async Task<UserExternalLoginEntity?> GetByUserAndProviderAsync(
        Guid userId,
        string provider,
        CancellationToken cancellationToken = default)
    {
        return await context.Set<UserExternalLoginEntity>()
            .FirstOrDefaultAsync(
                e => e.UserId == userId && e.Provider == provider,
                cancellationToken);
    }

    public async Task AddAsync(
        UserExternalLoginEntity entity,
        CancellationToken cancellationToken = default)
    {
        await context.Set<UserExternalLoginEntity>().AddAsync(entity, cancellationToken);
    }

    public void Remove(UserExternalLoginEntity entity)
    {
        context.Set<UserExternalLoginEntity>().Remove(entity);
    }
}
