namespace Application.DTOs.Sessions
{
    public class SessionDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? Device { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime? LastUsedAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public Guid SessionStatusId { get; set; }
    }
}
