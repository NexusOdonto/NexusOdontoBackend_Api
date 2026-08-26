using Domain.Common;
using Domain.Entities.Security;

namespace Domain.Entities.People;

public class User : BaseEntity
{
    public Guid PersonId { get; set; }
    public string PasswordHash { get; set; } = null!;
    public DateTime? LastAccessAt { get; set; }

    public Person? Person { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = [];

    private User()
    {
    }

    public User(Guid personId, string passwordHash)
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
