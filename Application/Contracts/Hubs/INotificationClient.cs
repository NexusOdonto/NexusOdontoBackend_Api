using Application.DTOs.Notifications.HubNotification;
using Application.DTOs.Presence;

namespace Application.Contracts.Hubs;

public interface INotificationClient
{
    Task ReceiveNotification(NotificacionHubResponseDto notificacion);

    Task UserOnline(PresenceUserDto user);

    Task UserOffline(PresenceUserDto user);

    Task PresenceSnapshot(IReadOnlyList<PresenceUserDto> users);
}
