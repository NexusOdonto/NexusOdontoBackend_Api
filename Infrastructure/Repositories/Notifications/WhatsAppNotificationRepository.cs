using Application.Contracts.Repositories.Notifications;
using Domain.Entities.Notifications;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Notifications;

public sealed class WhatsAppNotificationRepository : Common.Repository<WhatsAppNotificationEntity>, IWhatsAppNotificationRepository
{
    public WhatsAppNotificationRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<WhatsAppNotificationEntity>> GetByTypeExcludingStatusAsync(
        Guid typeId,
        Guid excludedStatusId,
        CancellationToken cancellationToken = default)
    {
        return await Set
            .Where(n =>
                n.WhatsAppNotificationTypeId == typeId &&
                n.WhatsAppNotificationStatusId != excludedStatusId &&
                n.AppointmentId != null)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<WhatsAppNotificationEntity>> GetByAppointmentAndTypeExcludingStatusAsync(
        IReadOnlyCollection<Guid> appointmentIds,
        Guid typeId,
        Guid excludedStatusId,
        CancellationToken cancellationToken = default)
    {
        if (appointmentIds.Count == 0)
        {
            return [];
        }

        var ids = appointmentIds.Distinct().ToList();
        return await Set
            .Where(n =>
                n.AppointmentId != null &&
                ids.Contains(n.AppointmentId.Value) &&
                n.WhatsAppNotificationTypeId == typeId &&
                n.WhatsAppNotificationStatusId != excludedStatusId)
            .ToListAsync(cancellationToken);
    }
}
