using System.Security.Cryptography;
using System.Text;

namespace Application.Services.Security;

internal static class RefreshTokenHelper
{
    public static string GenerateToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }

    public static string HashToken(string refreshToken)
    {
        return Convert.ToHexString(
            SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken)));
    }
}
