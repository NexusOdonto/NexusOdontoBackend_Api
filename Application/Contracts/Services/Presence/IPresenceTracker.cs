using Application.DTOs.Presence;

namespace Application.Contracts.Services.Presence;

public interface IPresenceTracker
{
    /// <summary>Registra una conexión. Devuelve true si el usuario pasó de offline a online.</summary>
    bool AddConnection(string connectionId, PresenceUserDto user);

    /// <summary>Quita una conexión. Devuelve el DTO y true si el usuario quedó totalmente offline.</summary>
    bool RemoveConnection(string connectionId, out PresenceUserDto? user);

    void UpsertUser(string connectionId, PresenceUserDto user);

    IReadOnlyList<PresenceUserDto> GetOnlineUsers();
}
