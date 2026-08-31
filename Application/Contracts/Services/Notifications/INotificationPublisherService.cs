using Application.DTOs.Notifications.HubNotification;

namespace Application.Contracts.Services.Notifications;

public interface INotificationPublisherService
{
    Task<NotificacionHubResponseDto> SendNotificationToUserAsync(Guid receptorUsuarioId, NotificacionDto alerta);
    Task<NotificacionHubResponseDto> SendNotificationToRoleAsync(Guid rolId, NotificacionDto alerta);
    Task<NotificacionHubResponseDto> SendNotificationToUserAsync(int receptorUsuarioId, NotificacionDto alerta);
    Task<NotificacionHubResponseDto> SendNotificationToRoleAsync(int rolId, NotificacionDto alerta);
}
