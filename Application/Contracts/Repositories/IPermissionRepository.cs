using Domain.Entities.Security;

namespace Application.Contracts.Repositories;

public interface IPermissionRepository : IRepository<Permission>
{
    Task<Permission?> GetByModuleAndActionAsync(
        string module,
        Guid actionPermissionId,
        CancellationToken cancellationToken = default);
}
