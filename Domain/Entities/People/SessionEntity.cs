using Domain.Common;
using Domain.Entities.Catalogs;

namespace Domain.Entities.People;

public class SessionEntity : Entity
{
    public Guid UserId { get; set; }
    public string RefreshTokenHash { get; set; } = null!;
    public string TokenFamily { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }
    public DateTime? LastUsedAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public Guid SessionStatusId { get; set; }

    public UserEntity? UserEntity { get; set; }
    public SessionStatusEntity? SessionStatusEntity { get; set; }

    private SessionEntity()
    {
    }

    public SessionEntity(
        Guid userId,
        string refreshTokenHash,
        string tokenFamily,
        DateTime expiresAt,
        Guid sessionStatusId)
    {
        UserId = userId;
        RefreshTokenHash = refreshTokenHash;
        TokenFamily = tokenFamily;
        ExpiresAt = expiresAt;
        SessionStatusId = sessionStatusId;
    }

    public void Update(
        Guid sessionStatusId,
        DateTime? lastUsedAt,
        DateTime? revokedAt)
    {
        SessionStatusId = sessionStatusId;
        LastUsedAt = lastUsedAt;
        RevokedAt = revokedAt;
    }
}
