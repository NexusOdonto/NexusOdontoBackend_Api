using Domain.Common;
using Domain.Entities.Security;

namespace Domain.Entities.People;

public class UserEntity : BaseEntity
{
    public Guid PersonId { get; set; }
    public string PasswordHash { get; set; } = null!;
    public DateTime? LastAccessAt { get; set; }

    public PersonEntity? PersonEntity { get; set; }
    public ICollection<UserRoleEntity> UserRoles { get; set; } = [];
    public ICollection<UserExternalLoginEntity> ExternalLogins { get; set; } = [];

    private UserEntity()
    {
    }

    public UserEntity(Guid personId, string passwordHash)
    {
        PersonId = personId;
        PasswordHash = passwordHash;
    }

    public void Update(string passwordHash, DateTime? lastAccessAt)
    {
        UpdatedAt = DateTime.UtcNow;
        PasswordHash = passwordHash;
        LastAccessAt = lastAccessAt;
    }

    public void RegisterAccess(DateTime accessAt)
    {
        UpdatedAt = DateTime.UtcNow;
        LastAccessAt = accessAt;
    }
}
