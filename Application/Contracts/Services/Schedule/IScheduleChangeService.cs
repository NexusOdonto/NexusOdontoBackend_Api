using Application.DTOs.Appointments;
using Domain.Entities.Notifications;

namespace Application.Contracts.Services.Schedule;

/// <summary>
/// "Requires reschedule" marks: a CAMBIO_HORARIO WhatsApp notification whose status is not RESUELTA.
/// </summary>
public interface IScheduleChangeService
{
    Task<IReadOnlyList<AffectedAppointmentDto>> BuildAffectedAsync(
        IReadOnlyCollection<Guid> appointmentIds,
        CancellationToken cancellationToken = default);

    /// <summary>Phone the bot should write to: patient's chat identifier if any, else Person.Phone.</summary>
    Task<string?> ResolveBotPhoneAsync(Guid patientId, string? personPhone, CancellationToken cancellationToken = default);

    Task<ScheduleChangeCatalogIds> GetCatalogIdsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<WhatsAppNotificationEntity>> GetOpenMarksAsync(
        IReadOnlyCollection<Guid> appointmentIds,
        CancellationToken cancellationToken = default);

    /// <summary>Marks open CAMBIO_HORARIO notifications as resolved. Does not save changes.</summary>
    Task<int> ResolveMarksAsync(
        IReadOnlyCollection<Guid> appointmentIds,
        string reason,
        bool detachAppointment = false,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<RequiresRescheduleAppointmentDto>> GetRequiresRescheduleAsync(
        CancellationToken cancellationToken = default);
}

public sealed record ScheduleChangeCatalogIds(
    Guid ScheduleChangeTypeId,
    Guid PendingStatusId,
    Guid SentStatusId,
    Guid FailedStatusId,
    Guid ResolvedStatusId);
