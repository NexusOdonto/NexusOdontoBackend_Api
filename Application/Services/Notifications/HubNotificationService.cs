using Application.Contracts.Repositories;
using Application.Contracts.Services.Notifications;
using Application.DTOs.Notifications.HubNotification;
using Domain.Entities.Catalogs;
using Domain.Entities.Notifications;

namespace Application.Services.Notifications;

public sealed class HubNotificationService(
    IRepository<HubNotification> repository,
    ICatalogRepository<HubNotificationStatus> statusCatalog,
    IUnitOfWork unitOfWork) : IHubNotificationService
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

    public async Task<object> CreateAsync(CreateHubNotificationRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var statusId = await ServiceWrite.ResolveCatalogIdAsync(statusCatalog, "PENDING", "NEW", "UNREAD", "CREATED");

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new HubNotification(
                request.HubNotificationTypeId,
                request.NotificationPriorityId,
                request.Title,
                request.Content,
                statusId,
                recipientUserId: request.RecipientUserId,
                recipientRoleId: request.RecipientRoleId,
                entityType: request.EntityType,
                entityId: request.EntityId);
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

    private static object Map(HubNotification e) => new
    {
        e.Id, e.SenderUserId, e.RecipientUserId, e.RecipientRoleId, e.HubNotificationTypeId,
        e.NotificationPriorityId, e.Title, e.Content, e.EntityType, e.EntityId,
        e.HubNotificationStatusId, e.CreatedAt, e.DeliveredAt, e.ReadAt
    };
}
