using System.Globalization;
using System.Text.Json;
using Application.Contracts.Repositories.Catalogs;
using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.Notifications;
using Application.Contracts.Repositories.People;
using Application.Contracts.Repositories.Schedule;
using Application.Contracts.Repositories.Security;
using Application.Contracts.Services.Chatbot;
using Application.Contracts.Services.Notifications;
using Application.Contracts.Services.Schedule;
using Application.DTOs.Appointments;
using Application.DTOs.Availabilities;
using Application.DTOs.Chats.ScheduleChange;
using Application.DTOs.Notifications.HubNotification;
using Application.Exceptions;
using Domain.Entities.Catalogs;
using Domain.Entities.Notifications;
using Domain.Entities.Schedule;
using Microsoft.Extensions.Logging;

namespace Application.Services.Schedule;

public sealed class AvailabilityService(
    IAvailabilityRepository repository,
    IProfessionalRepository professionalRepository,
    IAppointmentRepository appointmentRepository,
    IWhatsAppNotificationRepository notificationRepository,
    IScheduleChangeService scheduleChangeService,
    IChatbotClient chatbotClient,
    INotificationPublisherService notificationPublisher,
    IRoleRepository roleRepository,
    ICatalogRepository<HubNotificationTypeEntity> hubTypeCatalog,
    ICatalogRepository<NotificationPriorityEntity> priorityCatalog,
    IUnitOfWork unitOfWork,
    ILogger<AvailabilityService> logger) : IAvailabilityService
{
    private const int ImpactWindowDays = 90;
    private const int MaxParallelBotCalls = 4;
    private const string NoPhonePlaceholder = "SIN_TELEFONO";

    public async Task<IReadOnlyList<AffectedAppointmentDto>> GetImpactAsync(
        AvailabilityImpactRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var rows = ValidateRows(request.Rows);
        var professionalId = await ResolveProfessionalIdAsync(request.ProfessionalId);

        var (affected, _) = await FindAffectedAppointmentsAsync(professionalId, rows, cancellationToken);
        return await scheduleChangeService.BuildAffectedAsync(affected.Select(a => a.Id).ToList(), cancellationToken);
    }

    public async Task<ReplaceProfessionalAvailabilityResultDto> ReplaceForProfessionalAsync(
        Guid professionalId,
        ReplaceProfessionalAvailabilityDto request,
        Guid? requestedByUserId = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var rows = ValidateRows(request.Rows);
        var resolvedProfessionalId = await ResolveProfessionalIdAsync(professionalId);

        var (affectedEntities, upcoming) = await FindAffectedAppointmentsAsync(resolvedProfessionalId, rows, cancellationToken);
        var affectedIds = affectedEntities.Select(a => a.Id).ToList();
        var affected = await scheduleChangeService.BuildAffectedAsync(affectedIds, cancellationToken);
        var catalogIds = await scheduleChangeService.GetCatalogIdsAsync(cancellationToken);

        var botPhones = new Dictionary<Guid, string?>();
        foreach (var item in affected)
        {
            botPhones[item.AppointmentId] = await scheduleChangeService.ResolveBotPhoneAsync(
                item.PatientId, item.PatientPhone, cancellationToken);
        }

        var nowUtc = DateTime.UtcNow;
        var marks = new Dictionary<Guid, WhatsAppNotificationEntity>();
        var savedRows = new List<AvailabilityEntity>();

        await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var current = await repository.GetByProfessionalAsync(resolvedProfessionalId, cancellationToken);
            foreach (var old in current.Where(a => a.IsActive))
            {
                old.Update(old.StartDay, old.EndDay, old.StartTime, old.LunchStartTime, old.LunchEndTime, old.EndTime, false);
                repository.Update(old);
            }

            foreach (var row in rows)
            {
                var entity = new AvailabilityEntity(
                    resolvedProfessionalId,
                    row.StartDay,
                    row.EndDay,
                    row.StartTime,
                    row.LunchStartTime,
                    row.LunchEndTime,
                    row.EndTime);
                await repository.AddAsync(entity);
                savedRows.Add(entity);
            }

            // Previously marked appointments that fit the new schedule no longer need rescheduling;
            // still-affected ones get a fresh mark below.
            var upcomingIds = upcoming.Select(a => a.Id).ToList();
            await scheduleChangeService.ResolveMarksAsync(upcomingIds, "horario actualizado", cancellationToken: cancellationToken);

            foreach (var item in affected)
            {
                var phone = botPhones[item.AppointmentId];
                var mark = new WhatsAppNotificationEntity(
                    catalogIds.ScheduleChangeTypeId,
                    catalogIds.PendingStatusId,
                    Truncate(string.IsNullOrWhiteSpace(phone) ? NoPhonePlaceholder : phone, 30),
                    BuildMarkContent(item),
                    nowUtc,
                    appointmentId: item.AppointmentId,
                    patientId: item.PatientId,
                    templateName: "schedule_change",
                    templateParameters: JsonSerializer.Serialize(new
                    {
                        professionalId = item.ProfessionalId,
                        startsAt = item.StartsAt.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                        requestedByUserId
                    }),
                    provider: "chatbot");
                await notificationRepository.AddAsync(mark);
                marks[item.AppointmentId] = mark;
            }
        });

        var notified = 0;
        var failed = 0;

        if (request.NotifyPatients && affected.Count > 0)
        {
            (notified, failed) = await NotifyPatientsAsync(affected, botPhones, marks, catalogIds, cancellationToken);
        }

        if (affected.Count > 0)
        {
            await PublishReceptionSummaryAsync(affected, notified, failed, request.NotifyPatients, requestedByUserId);
        }

        return new ReplaceProfessionalAvailabilityResultDto
        {
            Rows = savedRows.Select(Map).ToList(),
            Affected = affected,
            Notified = notified,
            Failed = failed
        };
    }

    private async Task<(List<AppointmentEntity> Affected, List<AppointmentEntity> Upcoming)> FindAffectedAppointmentsAsync(
        Guid professionalId,
        IReadOnlyList<AvailabilityRowDto> rows,
        CancellationToken cancellationToken)
    {
        var now = AppointmentService.GetColombiaNow();
        var inRange = await appointmentRepository.GetByProfessionalInRangeAsync(
            professionalId, now, now.AddDays(ImpactWindowDays), cancellationToken);

        var upcoming = inRange
            .Where(a =>
                a.StartsAt >= now &&
                a.CancelledAt == null &&
                (a.AppointmentStatusId == AppointmentService.StatusScheduled ||
                 a.AppointmentStatusId == AppointmentService.StatusConfirmed))
            .OrderBy(a => a.StartsAt)
            .ToList();

        var affected = upcoming
            .Where(a => !rows.Any(r => AvailabilityRules.IsWithinAvailability(
                a.StartsAt, a.EndsAt, r.StartDay, r.EndDay, r.StartTime, r.LunchStartTime, r.LunchEndTime, r.EndTime)))
            .ToList();

        return (affected, upcoming);
    }

    private async Task<(int Notified, int Failed)> NotifyPatientsAsync(
        IReadOnlyList<AffectedAppointmentDto> affected,
        IReadOnlyDictionary<Guid, string?> botPhones,
        IReadOnlyDictionary<Guid, WhatsAppNotificationEntity> marks,
        ScheduleChangeCatalogIds catalogIds,
        CancellationToken cancellationToken)
    {
        using var gate = new SemaphoreSlim(MaxParallelBotCalls);

        var calls = affected.Select(async item =>
        {
            var phone = botPhones[item.AppointmentId];
            if (string.IsNullOrWhiteSpace(phone))
            {
                return (item.AppointmentId, Result: new ScheduleChangeNotifyResult
                {
                    Success = false,
                    Error = "El paciente no tiene teléfono ni conversación de WhatsApp registrada."
                });
            }

            await gate.WaitAsync(cancellationToken);
            try
            {
                var result = await chatbotClient.NotifyScheduleChangeAsync(new ScheduleChangeNotificationDto
                {
                    AppointmentId = item.AppointmentId,
                    PatientId = item.PatientId,
                    PatientName = item.PatientName,
                    Phone = phone,
                    ServiceId = item.ServiceId,
                    Service = item.ServiceName,
                    ProfessionalId = item.ProfessionalId,
                    Professional = item.ProfessionalName,
                    StartsAt = item.StartsAt,
                    EndsAt = item.EndsAt
                }, cancellationToken);
                return (item.AppointmentId, Result: result);
            }
            finally
            {
                gate.Release();
            }
        });

        var results = await Task.WhenAll(calls);

        var notified = 0;
        var failed = 0;
        var nowUtc = DateTime.UtcNow;

        foreach (var (appointmentId, result) in results)
        {
            var mark = marks[appointmentId];
            if (result.Success)
            {
                notified++;
                var queued = result.Status == "queued";
                mark.Update(
                    queued ? catalogIds.PendingStatusId : catalogIds.SentStatusId,
                    queued ? null : nowUtc,
                    mark.DeliveredAt,
                    mark.ReadAt,
                    null,
                    mark.ProviderMessageId,
                    result.Status,
                    null);
            }
            else
            {
                failed++;
                mark.Update(
                    catalogIds.FailedStatusId,
                    mark.SentAt,
                    mark.DeliveredAt,
                    mark.ReadAt,
                    nowUtc,
                    mark.ProviderMessageId,
                    result.Status,
                    Truncate(result.Error ?? "Error desconocido", 1000));
            }

            notificationRepository.Update(mark);
        }

        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "No se pudo guardar el estado de los avisos de cambio de horario.");
        }

        return (notified, failed);
    }

    private async Task PublishReceptionSummaryAsync(
        IReadOnlyList<AffectedAppointmentDto> affected,
        int notified,
        int failed,
        bool notifyPatients,
        Guid? requestedByUserId)
    {
        try
        {
            var reception = await roleRepository.GetByCodeAsync("RECEPCIONISTA");
            if (reception is null) return;

            var hubType = await hubTypeCatalog.GetByCodeAsync("CITA_CANCELADA")
                ?? (await hubTypeCatalog.GetAllAsync()).FirstOrDefault();
            var priority = await priorityCatalog.GetByCodeAsync("ALTA")
                ?? (await priorityCatalog.GetAllAsync()).FirstOrDefault();
            if (hubType is null || priority is null) return;

            var professionalName = affected[0].ProfessionalName;
            var count = affected.Count;
            var content = notifyPatients
                ? $"El horario de {professionalName} cambió y {count} cita(s) quedaron por fuera. Avisadas por WhatsApp: {notified}. Con error: {failed}. Revísalas en \"Requiere reprogramación\"."
                : $"El horario de {professionalName} cambió y {count} cita(s) quedaron por fuera sin aviso al paciente. Revísalas en \"Requiere reprogramación\".";

            await notificationPublisher.SendNotificationToRoleAsync(reception.Id, new NotificacionDto
            {
                EmisorUsuarioId = requestedByUserId,
                HubNotificationTypeId = hubType.Id,
                NotificationPriorityId = priority.Id,
                Title = $"Cambio de horario: {count} cita(s) requieren reprogramación",
                Content = content,
                EntityType = "PROFESSIONAL_SCHEDULE",
                EntityId = affected[0].ProfessionalId
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "No se pudo notificar a recepción el cambio de horario.");
        }
    }

    private static List<AvailabilityRowDto> ValidateRows(IReadOnlyList<AvailabilityRowDto>? rows)
    {
        var list = rows?.ToList() ?? [];
        var errors = new Dictionary<string, string[]>();

        for (var i = 0; i < list.Count; i++)
        {
            var r = list[i];
            var messages = new List<string>();

            if (r.StartDay is < 1 or > 7 || r.EndDay is < 1 or > 7)
                messages.Add("startDay y endDay deben estar entre 1 (lunes) y 7 (domingo).");
            else if (r.StartDay > r.EndDay)
                messages.Add("startDay no puede ser mayor que endDay.");

            if (r.StartTime >= r.EndTime)
                messages.Add("startTime debe ser anterior a endTime.");

            if (r.LunchStartTime > r.LunchEndTime)
                messages.Add("lunchStartTime no puede ser posterior a lunchEndTime.");
            else if (r.LunchStartTime < r.LunchEndTime &&
                     (r.LunchStartTime < r.StartTime || r.LunchEndTime > r.EndTime))
                messages.Add("El almuerzo debe estar dentro de la jornada.");

            if (messages.Count > 0)
                errors[$"rows[{i}]"] = messages.ToArray();
        }

        if (errors.Count > 0)
            throw new ValidationException(errors);

        return list;
    }

    private static string BuildMarkContent(AffectedAppointmentDto item) =>
        $"Cambio de horario: la cita de {item.ServiceName} de {item.PatientName} con {item.ProfessionalName} " +
        $"el {item.StartsAt.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)} quedó fuera del nuevo horario.";

    private static string Truncate(string value, int max) =>
        value.Length <= max ? value : value[..max];

    public async Task<IReadOnlyList<AvailabilityResponseDto>> GetAllAsync()
    {
        var items = await repository.GetAllAsync();
        return items.Select(Map).ToList();
    }

    public async Task<AvailabilityResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<AvailabilityResponseDto> CreateAsync(CreateAvailabilityDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var professionalId = await ResolveProfessionalIdAsync(request.ProfessionalId);

        var startTime = ResolveTime(request.StartTime, new TimeOnly(8, 0));
        var lunchStartTime = ResolveTime(request.LunchStartTime, new TimeOnly(12, 0));
        var lunchEndTime = ResolveTime(request.LunchEndTime, new TimeOnly(12, 0));
        var endTime = ResolveTime(request.EndTime, new TimeOnly(17, 0));

        var startDay = request.StartDay is >= 1 and <= 7 ? request.StartDay : 1;
        var endDay = request.EndDay is >= 1 and <= 7 ? request.EndDay : 5;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new AvailabilityEntity(
                professionalId,
                startDay,
                endDay,
                startTime,
                lunchStartTime,
                lunchEndTime,
                endTime);

            await repository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<AvailabilityResponseDto?> UpdateAsync(Guid id, UpdateAvailabilityDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return null;

        var startTime = request.StartTime;
        var lunchStartTime = request.LunchStartTime;
        var lunchEndTime = request.LunchEndTime;
        var endTime = request.EndTime;

        var startDay = request.StartDay is >= 1 and <= 7 ? request.StartDay : entity.StartDay;
        var endDay = request.EndDay is >= 1 and <= 7 ? request.EndDay : entity.EndDay;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.Update(
                startDay,
                endDay,
                startTime,
                lunchStartTime,
                lunchEndTime,
                endTime,
                request.IsActive);

            repository.Update(entity);
            return Map(entity);
        });
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return false;
        await ServiceWrite.InTransactionAsync(unitOfWork, () =>
        {
            entity.Update(entity.StartDay, entity.EndDay, entity.StartTime, entity.LunchStartTime, entity.LunchEndTime, entity.EndTime, false);
            repository.Update(entity);
            return Task.CompletedTask;
        });
        return true;
    }

    private async Task<Guid> ResolveProfessionalIdAsync(Guid inputId)
    {
        // 1. Intentar directamente como ProfessionalId
        var prof = await professionalRepository.GetByIdAsync(inputId);
        if (prof is not null) return prof.Id;

        // 2. Intentar como EmployeeId
        prof = await professionalRepository.GetByEmployeeIdAsync(inputId);
        if (prof is not null) return prof.Id;

        // 3. Buscar en la lista general por PersonId o EmployeeId
        var all = await professionalRepository.GetAllAsync();
        prof = all.FirstOrDefault(p =>
            p.Id == inputId ||
            p.EmployeeId == inputId ||
            (p.EmployeeEntity != null && p.EmployeeEntity.PersonId == inputId));

        if (prof is not null) return prof.Id;

        throw new KeyNotFoundException($"No se encontró un profesional odontólogo activo correspondiente al ID '{inputId}'. Por favor verifique el ID del profesional.");
    }

    private static TimeOnly ResolveTime(TimeOnly time, TimeOnly fallback) =>
        time == default ? fallback : time;

    private static AvailabilityResponseDto Map(AvailabilityEntity e) => new()
    {
        Id = e.Id,
        ProfessionalId = e.ProfessionalId,
        StartDay = e.StartDay,
        EndDay = e.EndDay,
        StartTime = e.StartTime,
        LunchStartTime = e.LunchStartTime,
        LunchEndTime = e.LunchEndTime,
        EndTime = e.EndTime,
        IsActive = e.IsActive
    };
}
