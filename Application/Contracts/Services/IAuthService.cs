using Application.DTOs.Auth;

namespace Application.Contracts.Services;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(
        LoginRequestDto request,
        string? ipAddress = null,
        string? userAgent = null,
        CancellationToken cancellationToken = default);

    Task<AuthProfileDto?> GetProfileAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<AuthResponseDto?> RefreshAsync(
        RefreshTokenRequestDto request,
        CancellationToken cancellationToken = default);

    Task<bool> LogoutAsync(
        RefreshTokenRequestDto request,
        CancellationToken cancellationToken = default);

    Task<UserResponseDto> RegisterAsync(
        RegisterRequestDto request,
        CancellationToken cancellationToken = default);
}
