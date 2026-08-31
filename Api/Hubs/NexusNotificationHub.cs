using System.Security.Claims;
using Application.Contracts.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs;

[Authorize]
public class NexusNotificationHub : Hub<INotificationClient>
{
    private readonly ILogger<NexusNotificationHub> _logger;

    public NexusNotificationHub(ILogger<NexusNotificationHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier ?? Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        _logger.LogInformation(
            "Usuario conectado al NexusNotificationHub. ConnectionId: {ConnectionId}, UserId: {UserId}",
            Context.ConnectionId,
            userId);

        // Subscribir automáticamente la conexión a los grupos por rol del usuario
        var roleClaims = Context.User?.FindAll("roleId").Select(c => c.Value)
            .Concat(Context.User?.FindAll(ClaimTypes.Role).Select(c => c.Value) ?? Array.Empty<string>());

        if (roleClaims != null)
        {
            foreach (var roleId in roleClaims.Distinct())
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, roleId);
                _logger.LogDebug("Conexión {ConnectionId} agregada al grupo de rol {RoleId}", Context.ConnectionId, roleId);
            }
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier ?? Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (exception != null)
        {
            _logger.LogError(
                exception,
                "Conexión perdida con error en NexusNotificationHub. ConnectionId: {ConnectionId}, UserId: {UserId}",
                Context.ConnectionId,
                userId);
        }
        else
        {
            _logger.LogInformation(
                "Usuario desconectado de NexusNotificationHub. ConnectionId: {ConnectionId}, UserId: {UserId}",
                Context.ConnectionId,
                userId);
        }

        await base.OnDisconnectedAsync(exception);
    }
}
