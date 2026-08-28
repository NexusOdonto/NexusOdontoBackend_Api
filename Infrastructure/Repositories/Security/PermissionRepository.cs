using Application.Contracts.Repositories.Security;
using Domain.Entities.Security;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Security;

public sealed class PermissionRepository : Common.Repository<PermissionEntity>, IPermissionRepository
{
    public PermissionRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<PermissionEntity?> GetByModuleAndActionAsync(
        string module,
        Guid actionPermissionId,
        CancellationToken cancellationToken = default)
    {
        return await Set.FirstOrDefaultAsync(
            p => p.Module == module && p.ActionPermissionId == actionPermissionId,
            cancellationToken);
    }
}
