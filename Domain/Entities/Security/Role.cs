using Domain.Common;

namespace Domain.Entities.Security;

public class Role : BaseEntity
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public ICollection<Permission> Permissions { get; set; } = [];
    public ICollection<UserRole> UserRoles { get; set; } = [];

    private Role()
    {
    }

    public Role(string code, string name, string? description = null)
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
