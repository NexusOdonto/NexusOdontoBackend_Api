using Domain.Entities.People;

namespace Domain.Entities.Security;

public class UserExternalLoginEntity
{
    public Guid UserId { get; set; }
    public string Provider { get; set; } = null!;
    public string ProviderKey { get; set; } = null!;
    public string? ProviderEmail { get; set; }
    public DateTime LinkedAt { get; set; }

    public UserEntity? UserEntity { get; set; }

    private UserExternalLoginEntity()
    {
    }

    public UserExternalLoginEntity(
        Guid userId,
        string provider,
        string providerKey,
        string? providerEmail)
    {
        UserId = userId;
        Provider = provider;
        ProviderKey = providerKey;
        ProviderEmail = providerEmail;
        LinkedAt = DateTime.UtcNow;
    }
}
