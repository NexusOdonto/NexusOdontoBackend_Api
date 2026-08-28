using Application.Contracts.Repositories.Common;
using Domain.Entities.Security;

namespace Application.Contracts.Repositories.Security;

public interface IPermissionRepository : IRepository<PermissionEntity>
{
    Task<PermissionEntity?> GetByModuleAndActionAsync(
        string module,
        Guid actionPermissionId,
        CancellationToken cancellationToken = default);
}
