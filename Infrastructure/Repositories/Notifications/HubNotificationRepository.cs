using Application.Contracts.Repositories.Notifications;
using Domain.Entities.Notifications;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Notifications;

public sealed class HubNotificationRepository : Common.Repository<HubNotificationEntity>, IHubNotificationRepository
{
    public HubNotificationRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<HubNotificationEntity>> GetForRecipientAsync(
        Guid userId,
        IReadOnlyCollection<Guid> roleIds,
        DateTime sinceUtc,
        int take,
        CancellationToken cancellationToken = default)
    {
        var roles = roleIds.Distinct().ToList();
        return await Set
            .Where(n =>
                n.CreatedAt >= sinceUtc &&
                (n.RecipientUserId == userId ||
                 (n.RecipientRoleId != null && roles.Contains(n.RecipientRoleId.Value))))
            .OrderByDescending(n => n.CreatedAt)
            .Take(take)
            .ToListAsync(cancellationToken);
    }
}
