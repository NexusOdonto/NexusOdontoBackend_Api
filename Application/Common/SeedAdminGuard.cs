namespace Application.Common;

/// <summary>
/// Protects the seeded system administrator (AD001 / Carlos Administrator)
/// from soft-delete or deactivation via API.
/// IDs must stay in sync with Infrastructure AdminUserSeeder / EmployeeSeeder.
/// </summary>
public static class SeedAdminGuard
{
    public static readonly Guid AdminPersonId = new("1b000000-0000-0000-0000-000000000001");
    public static readonly Guid AdminUserId = new("1b000000-0000-0000-0000-000000000002");
    public static readonly Guid AdminEmployeeId = new("1c000000-0000-0000-0000-000000000001");

    public const string CannotDeactivateOrDeleteMessage =
        "El usuario administrador del sistema (AD001) no puede ser desactivado ni eliminado.";

    public static bool IsProtectedUser(Guid userId) => userId == AdminUserId;

    public static bool IsProtectedPerson(Guid personId) => personId == AdminPersonId;

    public static bool IsProtectedEmployee(Guid employeeId) => employeeId == AdminEmployeeId;
}
