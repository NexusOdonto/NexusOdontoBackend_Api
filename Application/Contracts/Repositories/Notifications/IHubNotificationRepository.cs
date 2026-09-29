using Application.Contracts.Repositories.Common;
using Domain.Entities.Notifications;

namespace Application.Contracts.Repositories.Notifications;

public interface IHubNotificationRepository : IRepository<HubNotificationEntity>
{
    Task<IReadOnlyList<HubNotificationEntity>> GetForRecipientAsync(
        Guid userId,
        IReadOnlyCollection<Guid> roleIds,
        DateTime sinceUtc,
        int take,
        CancellationToken cancellationToken = default);
}
