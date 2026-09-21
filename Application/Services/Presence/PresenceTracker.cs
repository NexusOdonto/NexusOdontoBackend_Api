using System.Collections.Concurrent;
using Application.Contracts.Services.Presence;
using Application.DTOs.Presence;

namespace Application.Services.Presence;

/// <summary>
/// Presencia en memoria del proceso API (sin BD / sin migraciones).
/// Si hay varias réplicas del API, cada una tendría su propio mapa;
/// en el despliegue actual (una instancia) es suficiente.
/// </summary>
public sealed class PresenceTracker : IPresenceTracker
{
    private readonly ConcurrentDictionary<string, PresenceUserDto> _byConnection = new(StringComparer.Ordinal);
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, byte>> _connectionsByUser =
        new(StringComparer.OrdinalIgnoreCase);
    private readonly object _gate = new();

    public bool AddConnection(string connectionId, PresenceUserDto user)
    {
        if (string.IsNullOrWhiteSpace(connectionId) || string.IsNullOrWhiteSpace(user.UserId))
        {
            return false;
        }

        user = Normalize(user);
        _byConnection[connectionId] = user;

        var userConnections = _connectionsByUser.GetOrAdd(
            user.UserId,
            _ => new ConcurrentDictionary<string, byte>(StringComparer.Ordinal));

        var isNewUserOnline = userConnections.IsEmpty;
        userConnections[connectionId] = 0;
        return isNewUserOnline;
    }

    public bool RemoveConnection(string connectionId, out PresenceUserDto? user)
    {
        user = null;
        if (!_byConnection.TryRemove(connectionId, out var removed))
        {
            return false;
        }

        user = removed;
        if (!_connectionsByUser.TryGetValue(removed.UserId, out var userConnections))
        {
            return true;
        }

        userConnections.TryRemove(connectionId, out _);
        if (!userConnections.IsEmpty)
        {
            return false;
        }

        _connectionsByUser.TryRemove(removed.UserId, out _);
        return true;
    }

    public void UpsertUser(string connectionId, PresenceUserDto user)
    {
        if (string.IsNullOrWhiteSpace(connectionId) || string.IsNullOrWhiteSpace(user.UserId))
        {
            return;
        }

        user = Normalize(user);
        _byConnection[connectionId] = user;

        var userConnections = _connectionsByUser.GetOrAdd(
            user.UserId,
            _ => new ConcurrentDictionary<string, byte>(StringComparer.Ordinal));
        userConnections[connectionId] = 0;
    }

    public IReadOnlyList<PresenceUserDto> GetOnlineUsers()
    {
        lock (_gate)
        {
            return _byConnection.Values
                .GroupBy(u => u.UserId, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First())
                .ToList();
        }
    }

    private static PresenceUserDto Normalize(PresenceUserDto user)
    {
        var keys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        void Add(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            var t = value.Trim();
            keys.Add(t);
            keys.Add(t.ToLowerInvariant());
            keys.Add(t.ToLowerInvariant().Replace(" ", string.Empty).Replace(".", string.Empty).Replace("-", string.Empty));
        }

        Add(user.UserId);
        Add(user.PersonId);
        Add(user.DocumentNumber);
        Add(user.Email);

        return new PresenceUserDto
        {
            UserId = user.UserId.Trim(),
            PersonId = string.IsNullOrWhiteSpace(user.PersonId) ? null : user.PersonId.Trim(),
            DocumentNumber = string.IsNullOrWhiteSpace(user.DocumentNumber) ? null : user.DocumentNumber.Trim(),
            Email = string.IsNullOrWhiteSpace(user.Email) ? null : user.Email.Trim().ToLowerInvariant(),
            Name = string.IsNullOrWhiteSpace(user.Name) ? null : user.Name.Trim(),
            Roles = user.Roles?.Where(r => !string.IsNullOrWhiteSpace(r)).Distinct(StringComparer.OrdinalIgnoreCase).ToArray()
                ?? Array.Empty<string>(),
            Keys = keys.ToArray()
        };
    }
}
