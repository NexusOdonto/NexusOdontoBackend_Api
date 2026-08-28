using Application.Contracts.Repositories.Common;
using Domain.Entities.People;

namespace Application.Contracts.Repositories.People;

public interface ISessionRepository : IRepository<SessionEntity>
{
    Task<SessionEntity?> GetByRefreshTokenHashAsync(
        string refreshTokenHash,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SessionEntity>> GetActiveByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}
