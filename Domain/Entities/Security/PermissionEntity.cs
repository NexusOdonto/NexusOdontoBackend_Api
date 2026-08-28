using Domain.Common;
using Domain.Entities.Catalogs;

namespace Domain.Entities.Security;

public class PermissionEntity : Entity
{
    public string Module { get; set; } = null!;
    public Guid ActionPermissionId { get; set; }
    public string? Description { get; set; }

    public ActionPermissionEntity? ActionPermissionEntity { get; set; }
    public ICollection<RoleEntity> Roles { get; set; } = [];

    private PermissionEntity()
    {
    }

    public PermissionEntity(string module, Guid actionPermissionId, string? description = null)
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
