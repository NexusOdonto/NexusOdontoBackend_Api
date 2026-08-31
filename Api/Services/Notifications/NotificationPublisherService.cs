using Api.Hubs;
using Application.Contracts.Hubs;
using Application.Contracts.Repositories.Catalogs;
using Application.Contracts.Repositories.Common;
using Application.Contracts.Services.Notifications;
using Application.DTOs.Notifications.HubNotification;
using Application.Services;
using Domain.Entities.Catalogs;
using Domain.Entities.Notifications;
using Microsoft.AspNetCore.SignalR;

namespace Api.Services.Notifications;

public class NotificationPublisherService(
    IHubContext<NexusNotificationHub, INotificationClient> hubContext,
    IRepository<HubNotificationEntity> notificationRepository,
    ICatalogRepository<HubNotificationStatusEntity> statusCatalogRepository,
    IUnitOfWork unitOfWork,
    ILogger<NotificationPublisherService> logger) : INotificationPublisherService
{
    public async Task<NotificacionHubResponseDto> SendNotificationToUserAsync(
        Guid receptorUsuarioId,
        NotificacionDto alerta)
    {
        ArgumentNullException.ThrowIfNull(alerta);

        var pendingStatusId = await ServiceWrite.ResolveCatalogIdAsync(
            statusCatalogRepository,
            "PENDING", "UNREAD", "CREATED");

        var entity = new HubNotificationEntity(
            hubNotificationTypeId: alerta.HubNotificationTypeId,
            notificationPriorityId: alerta.NotificationPriorityId,
            title: alerta.Title,
            content: alerta.Content,
            hubNotificationStatusId: pendingStatusId,
            senderUserId: alerta.EmisorUsuarioId,
            recipientUserId: receptorUsuarioId,
            entityType: alerta.EntityType,
            entityId: alerta.EntityId
        );

        await notificationRepository.AddAsync(entity);
        await unitOfWork.SaveChangesAsync();

        var responseDto = MapToResponse(entity);

        try
        {
            await hubContext.Clients.User(receptorUsuarioId.ToString())
                .ReceiveNotification(responseDto);

            logger.LogInformation(
                "Notificación {NotificationId} emitida en tiempo real al usuario {UserId}.",
                entity.Id, receptorUsuarioId);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error al emitir notificación {NotificationId} vía SignalR al usuario {UserId}.",
                entity.Id, receptorUsuarioId);
        }

        return responseDto;
    }

    public async Task<NotificacionHubResponseDto> SendNotificationToRoleAsync(
        Guid rolId,
        NotificacionDto alerta)
    {
        ArgumentNullException.ThrowIfNull(alerta);

        var pendingStatusId = await ServiceWrite.ResolveCatalogIdAsync(
            statusCatalogRepository,
            "PENDING", "UNREAD", "CREATED");

        var entity = new HubNotificationEntity(
            hubNotificationTypeId: alerta.HubNotificationTypeId,
            notificationPriorityId: alerta.NotificationPriorityId,
            title: alerta.Title,
            content: alerta.Content,
            hubNotificationStatusId: pendingStatusId,
            senderUserId: alerta.EmisorUsuarioId,
            recipientRoleId: rolId,
            entityType: alerta.EntityType,
            entityId: alerta.EntityId
        );

        await notificationRepository.AddAsync(entity);
        await unitOfWork.SaveChangesAsync();

        var responseDto = MapToResponse(entity);

        try
        {
            await hubContext.Clients.Group(rolId.ToString())
                .ReceiveNotification(responseDto);

            logger.LogInformation(
                "Notificación {NotificationId} emitida en tiempo real al grupo de rol {RoleId}.",
                entity.Id, rolId);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error al emitir notificación {NotificationId} vía SignalR al grupo de rol {RoleId}.",
                entity.Id, rolId);
        }

        return responseDto;
    }

    public Task<NotificacionHubResponseDto> SendNotificationToUserAsync(int receptorUsuarioId, NotificacionDto alerta)
    {
        throw new NotSupportedException("NexusOdonto utiliza identificadores Únicos (Guid) para los Usuarios.");
    }

    public Task<NotificacionHubResponseDto> SendNotificationToRoleAsync(int rolId, NotificacionDto alerta)
    {
        throw new NotSupportedException("NexusOdonto utiliza identificadores Únicos (Guid) para los Roles.");
    }

    private static NotificacionHubResponseDto MapToResponse(HubNotificationEntity entity) => new()
    {
        Id = entity.Id,
        SenderUserId = entity.SenderUserId,
        RecipientUserId = entity.RecipientUserId,
        RecipientRoleId = entity.RecipientRoleId,
        HubNotificationTypeId = entity.HubNotificationTypeId,
        NotificationPriorityId = entity.NotificationPriorityId,
        Title = entity.Title,
        Content = entity.Content,
        EntityType = entity.EntityType,
        EntityId = entity.EntityId,
        HubNotificationStatusId = entity.HubNotificationStatusId,
        CreatedAt = entity.CreatedAt,
        DeliveredAt = entity.DeliveredAt,
        ReadAt = entity.ReadAt
    };
}
