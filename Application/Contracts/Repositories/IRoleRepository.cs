using Domain.Entities.Security;

namespace Application.Contracts.Repositories;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default);

    Task<Role?> GetByIdWithPermissionsAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
