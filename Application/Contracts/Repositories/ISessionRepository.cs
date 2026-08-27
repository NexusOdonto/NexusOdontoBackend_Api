using Domain.Entities.People;

namespace Application.Contracts.Repositories;

public interface ISessionRepository : IRepository<Session>
{
    Task<Session?> GetByRefreshTokenHashAsync(
        string refreshTokenHash,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Session>> GetActiveByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}
