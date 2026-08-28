using Application.Contracts.Repositories.Security;
using Domain.Entities.Security;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Security;

public sealed class RoleRepository : Common.Repository<RoleEntity>, IRoleRepository
{
    public RoleRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<RoleEntity?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        return await Set.FirstOrDefaultAsync(r => r.Code == code, cancellationToken);
    }

    public async Task<RoleEntity?> GetByIdWithPermissionsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await Set
            .Include(r => r.Permissions)
                .ThenInclude(p => p.ActionPermissionEntity)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }
}
