namespace Application.DTOs.Auth
{
    public sealed class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;

        public DateTime ExpiresAtUtc { get; set; }

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime RefreshExpiresAtUtc { get; set; }

        public UserResponseDto User { get; set; } = new();

        public IReadOnlyList<string> Permissions { get; set; } = [];
    }
}
