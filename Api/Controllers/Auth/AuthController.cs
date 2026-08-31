using System.Security.Claims;
using Application.Contracts.Repositories.Security;
using Application.Contracts.Services;
using Application.DTOs.Auth;
using Application.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Auth;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(
    IAuthService authService,
    IUserExternalLoginRepository externalLoginRepository,
    AuthSettings authSettings) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(request, cancellationToken);

        if (result is null)
        {
            return Unauthorized(new { message = "Invalid document number or password." });
        }

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("google/login")]
    public IActionResult GoogleLogin()
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(GoogleCallback))
        };

        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [AllowAnonymous]
    [HttpGet("google/callback")]
    public async Task<IActionResult> GoogleCallback(CancellationToken cancellationToken)
    {
        var authenticateResult = await HttpContext.AuthenticateAsync("GoogleOAuth");

        if (!authenticateResult.Succeeded || authenticateResult.Principal is null)
        {
            return Redirect(BuildFrontendErrorUrl("Google authentication failed."));
        }

        var googleSub = authenticateResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? authenticateResult.Principal.FindFirstValue("sub");

        if (string.IsNullOrWhiteSpace(googleSub))
        {
            return Redirect(BuildFrontendErrorUrl("Google account identifier was not provided."));
        }

        var googleEmail = authenticateResult.Principal.FindFirstValue(ClaimTypes.Email)
            ?? authenticateResult.Principal.FindFirstValue("email");

        await HttpContext.SignOutAsync("GoogleOAuth");

        var externalLogin = await externalLoginRepository.GetByProviderKeyAsync(
            AuthService.GoogleProvider,
            googleSub,
            cancellationToken);

        if (externalLogin is not null)
        {
            var authResponse = await authService.SignInByExternalLoginAsync(
                externalLogin.UserId,
                cancellationToken);

            if (authResponse is null)
            {
                return Redirect(BuildFrontendErrorUrl("Linked account is inactive or not found."));
            }

            var successUrl = BuildFrontendSuccessUrl(authResponse.Token, authResponse.RefreshToken);
            return Redirect(successUrl);
        }

        var linkToken = authService.CreateGoogleLinkToken(googleSub, googleEmail);
        var linkUrl = $"{authSettings.FrontendLinkUrl}?linkToken={Uri.EscapeDataString(linkToken)}";
        return Redirect(linkUrl);
    }

    [AllowAnonymous]
    [HttpPost("google/link")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> LinkGoogle(
        [FromBody] GoogleLinkRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await authService.LinkGoogleAsync(request, cancellationToken);

        if (result is null)
        {
            return Unauthorized(new { message = "Invalid link token or credentials." });
        }

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh(
        [FromBody] RefreshTokenRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await authService.RefreshAsync(request, cancellationToken);

        if (result is null)
        {
            return Unauthorized(new { message = "Invalid or expired refresh token." });
        }

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Logout(
        [FromBody] RefreshTokenRequestDto request,
        CancellationToken cancellationToken)
    {
        var revoked = await authService.LogoutAsync(request, cancellationToken);

        if (!revoked)
        {
            return BadRequest(new { message = "Refresh token not found or already revoked." });
        }

        return NoContent();
    }

    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(typeof(AuthProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Me(CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var profile = await authService.GetProfileAsync(userId, cancellationToken);
        if (profile is null)
        {
            return Unauthorized();
        }

        return Ok(profile);
    }

    private string BuildFrontendSuccessUrl(string token, string refreshToken)
    {
        var separator = authSettings.FrontendSuccessUrl.Contains('?') ? '&' : '?';
        return $"{authSettings.FrontendSuccessUrl}{separator}token={Uri.EscapeDataString(token)}&refreshToken={Uri.EscapeDataString(refreshToken)}";
    }

    private string BuildFrontendErrorUrl(string message)
    {
        var separator = authSettings.FrontendSuccessUrl.Contains('?') ? '&' : '?';
        return $"{authSettings.FrontendSuccessUrl}{separator}error={Uri.EscapeDataString(message)}";
    }
}
