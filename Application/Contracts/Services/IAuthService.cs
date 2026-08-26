using Application.DTOs.Auth;

namespace Application.Contracts.Services;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(
        LoginRequestDto request,
        CancellationToken cancellationToken = default);

    Task<UserResponseDto> RegisterAsync(
        RegisterRequestDto request,
        CancellationToken cancellationToken = default);
}
