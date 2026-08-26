using Domain.Entities.People;

namespace Domain.Entities.Security;

public class UserRole
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    public Role? Role { get; set; }

    private UserRole()
    {
    }

    public UserRole(Guid userId, Guid roleId, DateTime? assignedAt = null)
    {
        UserId = userId;
        RoleId = roleId;
        AssignedAt = assignedAt ?? DateTime.UtcNow;
    }
}
