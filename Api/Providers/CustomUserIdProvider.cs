using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace Api.Providers;

public class CustomUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        // Extrae el ID del usuario desde ClaimTypes.NameIdentifier o claims estándar JWT
        return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? connection.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
            ?? connection.User?.FindFirst("sub")?.Value;
    }
}
