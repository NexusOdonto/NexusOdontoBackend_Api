using Application.Contracts.Repositories;
using Application.Contracts.Services.Notifications;
using Application.DTOs.Notifications.WhatsAppNotification;
using Domain.Entities.Catalogs;
using Domain.Entities.Notifications;

namespace Application.Services.Notifications;

public sealed class WhatsAppNotificationService(
    IRepository<WhatsAppNotification> repository,
    ICatalogRepository<WhatsAppNotificationStatus> statusCatalog,
    IUnitOfWork unitOfWork) : IWhatsAppNotificationService
{
    public async Task<IReadOnlyList<object>> GetAllAsync()
    {
        var items = await repository.GetAllAsync();
        return items.Select(Map).Cast<object>().ToList();
    }

    public async Task<object?> GetByIdAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<object> CreateAsync(CreateWhatsAppNotificationRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var statusId = await ServiceWrite.ResolveCatalogIdAsync(statusCatalog, "PENDING", "SCHEDULED", "QUEUED", "NEW");

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new WhatsAppNotification(
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

    private static object Map(WhatsAppNotification e) => new
    {
        e.Id, e.AppointmentId, e.PatientId, e.WhatsAppNotificationTypeId, e.WhatsAppNotificationStatusId,
        e.DestinationPhone, e.Content, e.TemplateName, e.TemplateParameters, e.ScheduledFor,
        e.SentAt, e.DeliveredAt, e.ReadAt, e.FailedAt
    };
}
