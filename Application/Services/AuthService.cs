using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.People;
using Application.Contracts.Services;
using Application.DTOs.Auth;
using Domain.Entities.People;
using Domain.Entities.Security;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public sealed class AuthService(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    JwtSettings jwtSettings) : IAuthService
{
    public async Task<AuthResponseDto?> LoginAsync(
        LoginRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        string? email;
        if (request.Login.Contains('@'))
        {
            email = request.Login;
        }
        else
        {
            email = await userRepository.GetEmailByDocumentNumberAsync(
                request.Login,
                cancellationToken);

            if (email is null)
            {
                return null;
            }
        }

        var user = await userRepository.GetByEmailWithRolesAsync(
            email,
            cancellationToken);

        if (user is null ||
            !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return null;
        }

        user.RegisterAccess(DateTime.UtcNow);
        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return GenerateToken(user);
    }

    public async Task<UserResponseDto> RegisterAsync(
        RegisterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = new UserEntity(request.PersonId, passwordHash);
        user.UserRoles.Add(new UserRoleEntity(user.Id, request.RoleId));

        await userRepository.AddAsync(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var loaded = await userRepository.GetByIdWithRolesAsync(user.Id, cancellationToken)
            ?? user;

        return MapToResponse(loaded);
    }

    private AuthResponseDto GenerateToken(UserEntity user)
    {
        ValidateJwtSettings();

        var expiresAtUtc = DateTime.UtcNow.AddMinutes(jwtSettings.ExpirationMinutes);
        var primaryRole = user.UserRoles.FirstOrDefault()?.RoleEntity;
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        if (primaryRole is not null)
        {
            claims.Add(new Claim("roleId", primaryRole.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Role, primaryRole.Name));
        }

        foreach (var userRole in user.UserRoles)
        {
            claims.Add(new Claim("roleId", userRole.RoleId.ToString()));
        }

        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.SigningKey));
        var credentials = new SigningCredentials(
            signingKey,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: expiresAtUtc,
            signingCredentials: credentials);

        return new AuthResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAtUtc = expiresAtUtc
        };
    }

    public static UserResponseDto MapToResponse(UserEntity user)
    {
        var person = user.PersonEntity;
        return new UserResponseDto
        {
            Id = user.Id,
            PersonId = user.PersonId,
            Name = person is null
                ? string.Empty
                : $"{person.FirstName} {person.LastName}".Trim(),
            Email = person?.Email ?? string.Empty,
            RoleIds = user.UserRoles.Select(ur => ur.RoleId).ToList()
        };
    }

    private void ValidateJwtSettings()
    {
        if (string.IsNullOrWhiteSpace(jwtSettings.Issuer) ||
            string.IsNullOrWhiteSpace(jwtSettings.Audience) ||
            string.IsNullOrWhiteSpace(jwtSettings.SigningKey))
        {
            throw new InvalidOperationException("JWT configuration is incomplete.");
        }

        if (Encoding.UTF8.GetByteCount(jwtSettings.SigningKey) < 32)
        {
            throw new InvalidOperationException(
                "The JWT signing key must contain at least 32 bytes.");
        }

        if (jwtSettings.ExpirationMinutes <= 0)
        {
            throw new InvalidOperationException(
                "JWT expiration must be greater than zero.");
        }
    }
}
