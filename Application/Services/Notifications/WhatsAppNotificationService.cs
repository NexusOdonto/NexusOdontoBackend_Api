using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.Catalogs;
using Application.Contracts.Services.Notifications;
using Application.DTOs.Notifications.WhatsAppNotification;
using Domain.Entities.Catalogs;
using Domain.Entities.Notifications;

namespace Application.Services.Notifications;

public sealed class WhatsAppNotificationService(
    IRepository<WhatsAppNotificationEntity> repository,
    ICatalogRepository<WhatsAppNotificationStatusEntity> statusCatalog,
    IUnitOfWork unitOfWork) : IWhatsAppNotificationService
{
    public async Task<IReadOnlyList<WhatsAppNotificationResponseDto>> GetAllAsync()
    {
        var items = await repository.GetAllAsync();
        return items.Select(Map).ToList();
    }

    public async Task<WhatsAppNotificationResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<WhatsAppNotificationResponseDto> CreateAsync(CreateWhatsAppNotificationRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var statusId = await ServiceWrite.ResolveCatalogIdAsync(statusCatalog, "PENDING", "SCHEDULED", "QUEUED", "NEW");

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new WhatsAppNotificationEntity(
                request.WhatsAppNotificationTypeId,
                statusId,
                request.DestinationPhone,
                request.Content,
                request.ScheduledFor,
                request.AppointmentId,
                request.PatientId,
                request.TemplateName,
                request.TemplateParameters);
            await repository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return false;
        await ServiceWrite.InTransactionAsync(unitOfWork, () => { repository.Remove(entity); return Task.CompletedTask; });
        return true;
    }

    private static WhatsAppNotificationResponseDto Map(WhatsAppNotificationEntity e) => new()
    {
        Id = e.Id,
        AppointmentId = e.AppointmentId,
        PatientId = e.PatientId,
        WhatsAppNotificationTypeId = e.WhatsAppNotificationTypeId,
        WhatsAppNotificationStatusId = e.WhatsAppNotificationStatusId,
        DestinationPhone = e.DestinationPhone,
        Content = e.Content,
        TemplateName = e.TemplateName,
        TemplateParameters = e.TemplateParameters,
        ScheduledFor = e.ScheduledFor,
        SentAt = e.SentAt,
        DeliveredAt = e.DeliveredAt,
        ReadAt = e.ReadAt,
        FailedAt = e.FailedAt
    };
}
