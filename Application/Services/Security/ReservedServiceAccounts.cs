namespace Application.Services.Security;

/// <summary>
/// Identifiers reserved for the BOT_SERVICE seed account (not for interactive humans).
/// </summary>
public static class ReservedServiceAccounts
{
    public static readonly Guid BotServiceRoleId = new("13000000-0000-0000-0000-000000000006");

    private static readonly HashSet<string> ReservedIdentifiers = new(StringComparer.OrdinalIgnoreCase)
    {
        "BOT001",
        "BOT-SERVICE-01",
        "bot_service@nexusodonto.com"
    };

    public static bool IsReservedIdentifier(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        return ReservedIdentifiers.Contains(value.Trim());
    }
}
