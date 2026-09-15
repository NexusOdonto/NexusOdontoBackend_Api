using Application.DTOs.Auth;

namespace Application.Contracts.Services;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(
        LoginRequestDto request,
        string? internalSecret = null,
        CancellationToken cancellationToken = default);

    Task<AuthResponseDto?> SignInByExternalLoginAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<AuthResponseDto?> LinkGoogleAsync(
        GoogleLinkRequestDto request,
        CancellationToken cancellationToken = default);

    string CreateGoogleLinkToken(string googleSub, string? googleEmail);

    GoogleLinkTokenClaims? ValidateGoogleLinkToken(string linkToken);

    Task<AuthProfileDto?> GetProfileAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<AuthResponseDto?> RefreshAsync(
        RefreshTokenRequestDto request,
        CancellationToken cancellationToken = default);

    Task<bool> LogoutAsync(
        RefreshTokenRequestDto request,
        CancellationToken cancellationToken = default);

    Task<bool> ChangePasswordAsync(
        Guid userId,
        string newPassword,
        CancellationToken cancellationToken = default);
}
