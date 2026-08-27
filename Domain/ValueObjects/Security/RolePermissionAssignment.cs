using Domain.Common;

namespace Domain.ValueObjects.Security;

public sealed record RolePermissionAssignment
{
    public Guid RoleId { get; }

    public Guid PermissionId { get; }

    private RolePermissionAssignment(Guid roleId, Guid permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }

    public static RolePermissionAssignment Create(
        Guid roleId,
        Guid permissionId)
    {
        if (roleId == Guid.Empty)
        {
            throw new DomainException("Role ID cannot be empty.");
        }

        if (permissionId == Guid.Empty)
        {
            throw new DomainException("Permission ID cannot be empty.");
        }

        return new RolePermissionAssignment(roleId, permissionId);
    }
}
