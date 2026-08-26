using Domain.Common;
using Domain.Entities.Catalogs;

namespace Domain.Entities.Security;

public class Permission : Entity
{
    public string Module { get; set; } = null!;
    public Guid ActionPermissionId { get; set; }
    public string? Description { get; set; }

    public ActionPermission? ActionPermission { get; set; }
    public ICollection<Role> Roles { get; set; } = [];

    private Permission()
    {
    }

    public Permission(string module, Guid actionPermissionId, string? description = null)
    {
        Module = module;
        ActionPermissionId = actionPermissionId;
        Description = description;
    }

    public void Update(string module, Guid actionPermissionId, string? description)
    {
        Module = module;
        ActionPermissionId = actionPermissionId;
        Description = description;
    }
}
