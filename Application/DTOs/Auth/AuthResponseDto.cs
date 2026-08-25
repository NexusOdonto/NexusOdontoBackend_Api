namespace Application.DTOs.Auth
{
    public sealed class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;

        public DateTime ExpiresAtUtc { get; set; }
    }
}
