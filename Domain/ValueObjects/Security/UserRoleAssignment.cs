using Domain.Common;

namespace Domain.ValueObjects.Security;

public sealed record UserRoleAssignment
{
    public Guid UserId { get; }

    public Guid RoleId { get; }

    public DateTime AssignedAt { get; }

    private UserRoleAssignment(
        Guid userId,
        Guid roleId,
        DateTime assignedAt)
    {
        UserId = userId;
        RoleId = roleId;
        AssignedAt = assignedAt;
    }

    public static UserRoleAssignment Create(
        Guid userId,
        Guid roleId,
        DateTime assignedAt)
    {
        if (userId == Guid.Empty)
        {
            throw new DomainException("User ID cannot be empty.");
        }

        if (roleId == Guid.Empty)
        {
            throw new DomainException("Role ID cannot be empty.");
        }

        if (assignedAt == default)
        {
            throw new DomainException("Assigned date cannot be empty.");
        }

        return new UserRoleAssignment(userId, roleId, assignedAt);
    }
}
