using Application.Contracts.Repositories.Catalogs;
using Application.Contracts.Repositories.Chatbot;
using Application.Contracts.Repositories.Notifications;
using Application.Contracts.Repositories.Schedule;
using Application.Contracts.Services.Schedule;
using Application.DTOs.Appointments;
using Domain.Entities.Catalogs;
using Domain.Entities.Notifications;
using Domain.Entities.People;

namespace Application.Services.Schedule;

public sealed class ScheduleChangeService(
    IAppointmentRepository appointmentRepository,
    IWhatsAppNotificationRepository notificationRepository,
    IChatbotConversationRepository conversationRepository,
    ICatalogRepository<WhatsAppNotificationTypeEntity> typeCatalog,
    ICatalogRepository<WhatsAppNotificationStatusEntity> statusCatalog) : IScheduleChangeService
{
    public const string ScheduleChangeTypeCode = "CAMBIO_HORARIO";
    public const string PendingStatusCode = "PENDIENTE";
    public const string SentStatusCode = "ENVIADA";
    public const string FailedStatusCode = "FALLIDA";
    public const string ResolvedStatusCode = "RESUELTA";

    private ScheduleChangeCatalogIds? _catalogIds;

    public async Task<ScheduleChangeCatalogIds> GetCatalogIdsAsync(CancellationToken cancellationToken = default)
    {
        if (_catalogIds is not null) return _catalogIds;

        var type = await typeCatalog.GetByCodeAsync(ScheduleChangeTypeCode, cancellationToken)
            ?? throw new InvalidOperationException($"WhatsApp notification type '{ScheduleChangeTypeCode}' is not seeded.");

        _catalogIds = new ScheduleChangeCatalogIds(
            type.Id,
            await RequireStatusAsync(PendingStatusCode, cancellationToken),
            await RequireStatusAsync(SentStatusCode, cancellationToken),
            await RequireStatusAsync(FailedStatusCode, cancellationToken),
            await RequireStatusAsync(ResolvedStatusCode, cancellationToken));
        return _catalogIds;
    }

    public async Task<IReadOnlyList<AffectedAppointmentDto>> BuildAffectedAsync(
        IReadOnlyCollection<Guid> appointmentIds,
        CancellationToken cancellationToken = default)
    {
        var appointments = await appointmentRepository.GetWithDetailsAsync(appointmentIds, cancellationToken);
        var result = new List<AffectedAppointmentDto>(appointments.Count);

        foreach (var a in appointments.OrderBy(a => a.StartsAt))
        {
            var patientPerson = a.PatientEntity?.PersonEntity;
            var doctorPerson = a.ProfessionalEntity?.EmployeeEntity?.PersonEntity;
            var patientPhone = patientPerson?.Phone;
            if (string.IsNullOrWhiteSpace(patientPhone))
            {
                patientPhone = await ResolveBotPhoneAsync(a.PatientId, null, cancellationToken);
            }

            result.Add(new AffectedAppointmentDto
            {
                AppointmentId = a.Id,
                PatientId = a.PatientId,
                PatientName = FullName(patientPerson, "Paciente"),
                PatientPhone = patientPhone,
                ServiceId = a.ServiceId,
                ServiceName = a.ServiceEntity?.Name ?? "Consulta",
                ProfessionalId = a.ProfessionalId,
                ProfessionalName = FullName(doctorPerson, "Profesional"),
                StartsAt = a.StartsAt,
                EndsAt = a.EndsAt,
                StatusCode = AppointmentService.GetEffectiveStatusCode(a)
            });
        }

        return result;
    }

    public async Task<string?> ResolveBotPhoneAsync(Guid patientId, string? personPhone, CancellationToken cancellationToken = default)
    {
        var conversation = await conversationRepository.GetLatestByPatientIdAsync(patientId, cancellationToken);
        if (!string.IsNullOrWhiteSpace(conversation?.ChatIdentifier))
        {
            return conversation.ChatIdentifier.Trim();
        }

        return string.IsNullOrWhiteSpace(personPhone) ? null : personPhone.Trim();
    }

    public async Task<IReadOnlyList<WhatsAppNotificationEntity>> GetOpenMarksAsync(
        IReadOnlyCollection<Guid> appointmentIds,
        CancellationToken cancellationToken = default)
    {
        if (appointmentIds.Count == 0) return [];
        var ids = await GetCatalogIdsAsync(cancellationToken);
        return await notificationRepository.GetByAppointmentAndTypeExcludingStatusAsync(
            appointmentIds, ids.ScheduleChangeTypeId, ids.ResolvedStatusId, cancellationToken);
    }

    public async Task<int> ResolveMarksAsync(
        IReadOnlyCollection<Guid> appointmentIds,
        string reason,
        bool detachAppointment = false,
        CancellationToken cancellationToken = default)
    {
        if (appointmentIds.Count == 0) return 0;

        var type = await typeCatalog.GetByCodeAsync(ScheduleChangeTypeCode, cancellationToken);
        var resolved = await statusCatalog.GetByCodeAsync(ResolvedStatusCode, cancellationToken);
        if (type is null || resolved is null) return 0;

        var marks = await notificationRepository.GetByAppointmentAndTypeExcludingStatusAsync(
            appointmentIds, type.Id, resolved.Id, cancellationToken);

        foreach (var mark in marks)
        {
            mark.WhatsAppNotificationStatusId = resolved.Id;
            mark.ProviderResponse = Truncate(
                $"{mark.ProviderResponse} | resuelta {DateTime.UtcNow:yyyy-MM-ddTHH:mm:ssZ}: {reason}".TrimStart(' ', '|'),
                1000);
            if (detachAppointment)
            {
                mark.AppointmentId = null;
            }

            notificationRepository.Update(mark);
        }

        return marks.Count;
    }

    public async Task<IReadOnlyList<RequiresRescheduleAppointmentDto>> GetRequiresRescheduleAsync(
        CancellationToken cancellationToken = default)
    {
        var ids = await GetCatalogIdsAsync(cancellationToken);
        var marks = await notificationRepository.GetByTypeExcludingStatusAsync(
            ids.ScheduleChangeTypeId, ids.ResolvedStatusId, cancellationToken);

        var latestByAppointment = marks
            .GroupBy(m => m.AppointmentId!.Value)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(m => m.ScheduledFor).First());

        var affected = await BuildAffectedAsync(latestByAppointment.Keys.ToList(), cancellationToken);

        return affected
            .Where(a => a.StatusCode is "AGENDADA" or "CONFIRMADA")
            .Select(a =>
            {
                var mark = latestByAppointment[a.AppointmentId];
                return new RequiresRescheduleAppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    PatientId = a.PatientId,
                    PatientName = a.PatientName,
                    PatientPhone = a.PatientPhone,
                    ServiceId = a.ServiceId,
                    ServiceName = a.ServiceName,
                    ProfessionalId = a.ProfessionalId,
                    ProfessionalName = a.ProfessionalName,
                    StartsAt = a.StartsAt,
                    EndsAt = a.EndsAt,
                    StatusCode = a.StatusCode,
                    NotificationStatus = StatusCodeFor(mark.WhatsAppNotificationStatusId, ids),
                    NotifiedAt = mark.SentAt
                };
            })
            .ToList();
    }

    private static string StatusCodeFor(Guid statusId, ScheduleChangeCatalogIds ids) => statusId switch
    {
        var id when id == ids.SentStatusId => SentStatusCode,
        var id when id == ids.FailedStatusId => FailedStatusCode,
        _ => PendingStatusCode
    };

    private async Task<Guid> RequireStatusAsync(string code, CancellationToken cancellationToken)
    {
        var status = await statusCatalog.GetByCodeAsync(code, cancellationToken)
            ?? throw new InvalidOperationException($"WhatsApp notification status '{code}' is not seeded.");
        return status.Id;
    }

    private static string FullName(PersonEntity? person, string fallback) =>
        person is null ? fallback : $"{person.FirstName} {person.LastName}".Trim();

    private static string Truncate(string value, int max) =>
        value.Length <= max ? value : value[..max];
}
