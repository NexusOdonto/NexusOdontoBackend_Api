using Domain.Common;

namespace Domain.Entities.Security;

public class RoleEntity : BaseEntity
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public ICollection<PermissionEntity> Permissions { get; set; } = [];
    public ICollection<UserRoleEntity> UserRoles { get; set; } = [];

    private RoleEntity()
    {
    }

    public RoleEntity(string code, string name, string? description = null)
    {
        Code = code;
        Name = name;
        Description = description;
    }

    public void Update(string name, string? description)
    {
        UpdatedAt = DateTime.UtcNow;
        Name = name;
        Description = description;
    }
}
