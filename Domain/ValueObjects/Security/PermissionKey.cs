using Domain.Common;

namespace Domain.ValueObjects.Security;

public sealed record PermissionKey
{
    public string Module { get; }

    public Guid ActionPermissionId { get; }

    private PermissionKey(string module, Guid actionPermissionId)
    {
        Module = module;
        ActionPermissionId = actionPermissionId;
    }

    public static PermissionKey Create(
        string module,
        Guid actionPermissionId)
    {
        if (string.IsNullOrWhiteSpace(module))
        {
            throw new DomainException("Permission module cannot be empty.");
        }

        var normalizedModule = module.Trim();

        if (normalizedModule.Length > 80)
        {
            throw new DomainException(
                "Permission module cannot exceed 80 characters.");
        }

        if (actionPermissionId == Guid.Empty)
        {
            throw new DomainException(
                "Action permission ID cannot be empty.");
        }

        return new PermissionKey(normalizedModule, actionPermissionId);
    }
}
