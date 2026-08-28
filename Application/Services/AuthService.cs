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

        var UserEntity = await userRepository.GetByEmailWithRolesAsync(
            email,
            cancellationToken);

        if (UserEntity is null ||
            !BCrypt.Net.BCrypt.Verify(request.Password, UserEntity.PasswordHash))
        {
            return null;
        }

        UserEntity.RegisterAccess(DateTime.UtcNow);
        userRepository.Update(UserEntity);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return GenerateToken(UserEntity);
    }

    public async Task<UserResponseDto> RegisterAsync(
        RegisterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var UserEntity = new UserEntity(request.PersonId, passwordHash);
        UserEntity.UserRoles.Add(new UserRoleEntity(UserEntity.Id, request.RoleId));

        await userRepository.AddAsync(UserEntity);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var loaded = await userRepository.GetByIdWithRolesAsync(UserEntity.Id, cancellationToken)
            ?? UserEntity;

        return MapToResponse(loaded);
    }

    private AuthResponseDto GenerateToken(UserEntity UserEntity)
    {
        ValidateJwtSettings();

        var expiresAtUtc = DateTime.UtcNow.AddMinutes(jwtSettings.ExpirationMinutes);
        var primaryRole = UserEntity.UserRoles.FirstOrDefault()?.RoleEntity;
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, UserEntity.Id.ToString()),
            new(ClaimTypes.NameIdentifier, UserEntity.Id.ToString())
        };

        if (primaryRole is not null)
        {
            claims.Add(new Claim("roleId", primaryRole.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Role, primaryRole.Name));
        }

        foreach (var UserRoleEntity in UserEntity.UserRoles)
        {
            claims.Add(new Claim("roleId", UserRoleEntity.RoleId.ToString()));
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

    public static UserResponseDto MapToResponse(UserEntity UserEntity)
    {
        var PersonEntity = UserEntity.PersonEntity;
        return new UserResponseDto
        {
            Id = UserEntity.Id,
            PersonId = UserEntity.PersonId,
            Name = PersonEntity is null
                ? string.Empty
                : $"{PersonEntity.FirstName} {PersonEntity.LastName}".Trim(),
            Email = PersonEntity?.Email ?? string.Empty,
            RoleIds = UserEntity.UserRoles.Select(ur => ur.RoleId).ToList()
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
