using System.Security.Claims;
using Application.Contracts.Hubs;
using Application.Contracts.Repositories.People;
using Application.Contracts.Services.Presence;
using Application.DTOs.Presence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs;

[Authorize]
public class NexusNotificationHub : Hub<INotificationClient>
{
    private readonly ILogger<NexusNotificationHub> _logger;
    private readonly IPresenceTracker _presenceTracker;
    private readonly IUserRepository _userRepository;

    public NexusNotificationHub(
        ILogger<NexusNotificationHub> logger,
        IPresenceTracker presenceTracker,
        IUserRepository userRepository)
    {
        _logger = logger;
        _presenceTracker = presenceTracker;
        _userRepository = userRepository;
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

        // Presencia: no debe romper notificaciones si falla la resolución de perfil
        try
        {
            var presence = await ResolvePresenceAsync(CancellationToken.None);
            if (presence is not null)
            {
                var becameOnline = _presenceTracker.AddConnection(Context.ConnectionId, presence);
                if (becameOnline)
                {
                    await Clients.Others.UserOnline(presence);
                }

                await Clients.Caller.PresenceSnapshot(_presenceTracker.GetOnlineUsers());
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "No se pudo registrar presencia para ConnectionId {ConnectionId}", Context.ConnectionId);
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

        try
        {
            if (_presenceTracker.RemoveConnection(Context.ConnectionId, out var presence) && presence is not null)
            {
                await Clients.Others.UserOffline(presence);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "No se pudo limpiar presencia para ConnectionId {ConnectionId}", Context.ConnectionId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Permite al cliente refrescar claves de presencia (doc/email/personId) sin reiniciar el hub.
    /// </summary>
    public async Task SyncPresence()
    {
        var presence = await ResolvePresenceAsync(CancellationToken.None);
        if (presence is null)
        {
            return;
        }

        _presenceTracker.UpsertUser(Context.ConnectionId, presence);
        await Clients.Others.UserOnline(presence);
        await Clients.Caller.PresenceSnapshot(_presenceTracker.GetOnlineUsers());
    }

    public Task<IReadOnlyList<PresenceUserDto>> GetOnlineUsers()
    {
        return Task.FromResult(_presenceTracker.GetOnlineUsers());
    }

    private async Task<PresenceUserDto?> ResolvePresenceAsync(CancellationToken cancellationToken)
    {
        var userId =
            Context.UserIdentifier
            ?? Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? Context.User?.FindFirst("sub")?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return null;
        }

        var roles = Context.User?.FindAll(ClaimTypes.Role).Select(c => c.Value).Distinct().ToArray()
            ?? Array.Empty<string>();

        var personId = Context.User?.FindFirst("personId")?.Value;
        var documentNumber = Context.User?.FindFirst("documentNumber")?.Value;
        var email =
            Context.User?.FindFirst(ClaimTypes.Email)?.Value
            ?? Context.User?.FindFirst("email")?.Value;
        var name = Context.User?.FindFirst(ClaimTypes.Name)?.Value;

        // Tokens antiguos sin claims de persona: resolver desde BD (una vez al conectar)
        if (string.IsNullOrWhiteSpace(personId)
            || string.IsNullOrWhiteSpace(documentNumber)
            || string.IsNullOrWhiteSpace(email))
        {
            if (Guid.TryParse(userId, out var userGuid))
            {
                var user = await _userRepository.GetByIdWithRolesAsync(userGuid, cancellationToken);
                var person = user?.PersonEntity;
                if (user is not null && person is not null)
                {
                    personId ??= user.PersonId.ToString();
                    documentNumber ??= person.DocumentNumber;
                    email ??= person.Email;
                    name ??= $"{person.FirstName} {person.LastName}".Trim();
                    if (roles.Length == 0 && user.UserRoles.Count > 0)
                    {
                        roles = user.UserRoles
                            .Where(ur => ur.RoleEntity is not null)
                            .Select(ur => ur.RoleEntity!.Name)
                            .Distinct()
                            .ToArray();
                    }
                }
            }
        }

        return new PresenceUserDto
        {
            UserId = userId,
            PersonId = personId,
            DocumentNumber = documentNumber,
            Email = email,
            Name = name,
            Roles = roles
        };
    }
}
