namespace Application.Contracts.Services;

public sealed class JwtSettings
{
    public const int DefaultExpirationMinutes = 15;

    /// <summary>
    /// Fallback when Jwt:RefreshTokenExpirationDays is missing or &lt;= 0 after binding
    /// (configuration binders can overwrite property defaults with 0). Prefer setting
    /// Jwt__RefreshTokenExpirationDays=7 (dotnet run) or JWT_REFRESH_TOKEN_EXPIRATION_DAYS=7 (Compose).
    /// </summary>
    public const int DefaultRefreshTokenExpirationDays = 7;

    public string Issuer { get; init; } = string.Empty;

    public string Audience { get; init; } = string.Empty;

    public string SigningKey { get; init; } = string.Empty;

    public int ExpirationMinutes { get; init; } = DefaultExpirationMinutes;

    public int RefreshTokenExpirationDays { get; init; } = DefaultRefreshTokenExpirationDays;

    /// <summary>
    /// Reconstructs settings so ExpirationMinutes and RefreshTokenExpirationDays are always &gt; 0.
    /// Binders often leave those fields at 0 when the Jwt section exists but omits them;
    /// applying defaults here keeps login from throwing 500 at token issuance.
    /// </summary>
    public static JwtSettings Normalize(JwtSettings source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var expirationMinutes = source.ExpirationMinutes > 0
            ? source.ExpirationMinutes
            : DefaultExpirationMinutes;
        var refreshTokenExpirationDays = source.RefreshTokenExpirationDays > 0
            ? source.RefreshTokenExpirationDays
            : DefaultRefreshTokenExpirationDays;

        if (expirationMinutes <= 0 || refreshTokenExpirationDays <= 0)
        {
            throw new InvalidOperationException(
                "JWT expiration settings must be greater than zero after applying defaults. " +
                "Set Jwt__ExpirationMinutes and Jwt__RefreshTokenExpirationDays " +
                "(or JwtSettings:* / JWT_* Compose equivalents).");
        }

        return new JwtSettings
        {
            Issuer = source.Issuer,
            Audience = source.Audience,
            SigningKey = source.SigningKey,
            ExpirationMinutes = expirationMinutes,
            RefreshTokenExpirationDays = refreshTokenExpirationDays
        };
    }
}
