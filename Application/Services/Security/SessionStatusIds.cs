namespace Application.Services.Security;

internal static class SessionStatusIds
{
    public static readonly Guid Active = new("15000000-0000-0000-0000-000000000001");

    public static readonly Guid Revoked = new("15000000-0000-0000-0000-000000000002");
}
