using Domain.Entities.People;
using Domain.Entities.Security;

namespace Application.Services.Security;

public static class PermissionChecker
{
    public static IReadOnlyList<string> GetEffectivePermissions(UserEntity user)
    {
        ArgumentNullException.ThrowIfNull(user);

        return user.UserRoles
            .SelectMany(ur => ur.RoleEntity?.Permissions ?? [])
            .Select(FormatPermission)
            .Where(static key => key.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(static key => key, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public static bool HasPermission(UserEntity user, string module, string actionCode)
    {
        ArgumentNullException.ThrowIfNull(user);

        if (string.IsNullOrWhiteSpace(module) || string.IsNullOrWhiteSpace(actionCode))
        {
            return false;
        }

        // Full system access for Administrator role only
        if (user.UserRoles.Any(ur => ur.RoleEntity?.Code is "ADMINISTRADOR"))
        {
            return true;
        }

        var normalizedModule = module.Trim().ToUpperInvariant();
        var normalizedAction = actionCode.Trim().ToUpperInvariant();

        return user.UserRoles
            .SelectMany(ur => ur.RoleEntity?.Permissions ?? [])
            .Any(permission =>
                string.Equals(permission.Module, normalizedModule, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(
                    permission.ActionPermissionEntity?.Code,
                    normalizedAction,
                    StringComparison.OrdinalIgnoreCase));
    }

    public static string FormatPermission(PermissionEntity permission)
    {
        ArgumentNullException.ThrowIfNull(permission);

        var actionCode = permission.ActionPermissionEntity?.Code;
        if (string.IsNullOrWhiteSpace(actionCode))
        {
            return string.Empty;
        }

        return $"{permission.Module.Trim().ToUpperInvariant()}:{actionCode.Trim().ToUpperInvariant()}";
    }
}
