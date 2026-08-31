using Application.DTOs.Notifications.HubNotification;

namespace Application.Contracts.Hubs;

public interface INotificationClient
{
    Task ReceiveNotification(NotificacionHubResponseDto notificacion);
}
