using Application.Contracts.Repositories.People;
using Domain.Entities.People;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.People;

public sealed class SessionRepository : Common.Repository<SessionEntity>, ISessionRepository
{
    public SessionRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<SessionEntity?> GetByRefreshTokenHashAsync(
        string refreshTokenHash,
        CancellationToken cancellationToken = default)
    {
        return await Set.FirstOrDefaultAsync(
            s => s.RefreshTokenHash == refreshTokenHash,
            cancellationToken);
    }

    public async Task<IReadOnlyList<SessionEntity>> GetActiveByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        return await Set
            .Where(s => s.UserId == userId && s.RevokedAt == null && s.ExpiresAt > now)
            .ToListAsync(cancellationToken);
    }
}
