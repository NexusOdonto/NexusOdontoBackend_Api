using Domain.Entities.People;

namespace Domain.Entities.Security;

public class UserRoleEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    public UserEntity? UserEntity { get; set; }
    public RoleEntity? RoleEntity { get; set; }

    private UserRoleEntity()
    {
    }

    public UserRoleEntity(Guid userId, Guid roleId, DateTime? assignedAt = null)
    {
        UserId = userId;
        RoleId = roleId;
        AssignedAt = assignedAt ?? DateTime.UtcNow;
    }
}
