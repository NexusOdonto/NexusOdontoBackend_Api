using Application.Contracts.Repositories.Common;
using Domain.Entities.Notifications;

namespace Application.Contracts.Repositories.Notifications;

public interface IWhatsAppNotificationRepository : IRepository<WhatsAppNotificationEntity>
{
    Task<IReadOnlyList<WhatsAppNotificationEntity>> GetByTypeExcludingStatusAsync(
        Guid typeId,
        Guid excludedStatusId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<WhatsAppNotificationEntity>> GetByAppointmentAndTypeExcludingStatusAsync(
        IReadOnlyCollection<Guid> appointmentIds,
        Guid typeId,
        Guid excludedStatusId,
        CancellationToken cancellationToken = default);
}
