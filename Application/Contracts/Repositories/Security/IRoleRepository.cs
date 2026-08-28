using Application.Contracts.Repositories.Common;
using Domain.Entities.Security;

namespace Application.Contracts.Repositories.Security;

public interface IRoleRepository : IRepository<RoleEntity>
{
    Task<RoleEntity?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default);

    Task<RoleEntity?> GetByIdWithPermissionsAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
