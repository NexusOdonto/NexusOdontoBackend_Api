using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.Catalogs;
using Application.Contracts.Services.Notifications;
using Application.DTOs.Notifications.HubNotification;
using Domain.Entities.Catalogs;
using Domain.Entities.Notifications;

namespace Application.Services.Notifications;

public sealed class HubNotificationService(
    IRepository<HubNotificationEntity> repository,
    ICatalogRepository<HubNotificationStatusEntity> statusCatalog,
    IUnitOfWork unitOfWork) : IHubNotificationService
{
    public async Task<IReadOnlyList<HubNotificationResponseDto>> GetAllAsync()
    {
        var items = await repository.GetAllAsync();
        return items.Select(Map).ToList();
    }

    public async Task<HubNotificationResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<HubNotificationResponseDto> CreateAsync(CreateHubNotificationRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var statusId = await ServiceWrite.ResolveCatalogIdAsync(statusCatalog, "PENDING", "NEW", "UNREAD", "CREATED");

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new HubNotificationEntity(
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

    private static HubNotificationResponseDto Map(HubNotificationEntity e) => new()
    {
        Id = e.Id,
        SenderUserId = e.SenderUserId,
        RecipientUserId = e.RecipientUserId,
        RecipientRoleId = e.RecipientRoleId,
        HubNotificationTypeId = e.HubNotificationTypeId,
        NotificationPriorityId = e.NotificationPriorityId,
        Title = e.Title,
        Content = e.Content,
        EntityType = e.EntityType,
        EntityId = e.EntityId,
        HubNotificationStatusId = e.HubNotificationStatusId,
        CreatedAt = e.CreatedAt,
        DeliveredAt = e.DeliveredAt,
        ReadAt = e.ReadAt
    };
}
