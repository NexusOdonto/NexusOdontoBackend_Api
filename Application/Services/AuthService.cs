using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Contracts.Repositories;
using Application.Contracts.Services;
using Application.DTOs.Auth;
using Application.Exceptions;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public sealed class AuthService(
    IUnitOfWork unitOfWork,
    JwtSettings jwtSettings) : IAuthService
{
    public async Task<AuthResponseDto?> LoginAsync(
        LoginRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var usuario = await unitOfWork.Usuarios.GetByCorreoWithRoleAsync(
            request.Correo,
            cancellationToken);

        if (usuario is null ||
            !BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash.Value))
        {
            return null;
        }

        return GenerateToken(usuario);
    }

    public async Task<UsuarioResponseDto> RegisterAsync(
        RegisterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var usuarioExistente = await unitOfWork.Usuarios.GetByCorreoWithRoleAsync(
            request.Correo,
            cancellationToken);

        if (usuarioExistente is not null)
        {
            throw new ValidationException("Ya existe un usuario registrado con ese correo.");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var usuario = new Usuario(
            request.Nombre,
            request.Correo,
            passwordHash,
            request.RolId);

        await unitOfWork.Usuarios.AddAsync(usuario);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new UsuarioResponseDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre.Value,
            Correo = usuario.Correo.Value,
            RolId = usuario.RolId
        };
    }

    private AuthResponseDto GenerateToken(Usuario usuario)
    {
        ValidateJwtSettings();

        var expiresAtUtc = DateTime.UtcNow.AddMinutes(jwtSettings.ExpirationMinutes);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim("rolId", usuario.RolId.ToString()),
            new Claim(ClaimTypes.Role, usuario.Rol.Nombre.Value)
        };

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

    private void ValidateJwtSettings()
    {
        if (string.IsNullOrWhiteSpace(jwtSettings.Issuer) ||
            string.IsNullOrWhiteSpace(jwtSettings.Audience) ||
            string.IsNullOrWhiteSpace(jwtSettings.SigningKey))
        {
            throw new InvalidOperationException("La configuración JWT está incompleta.");
        }

        if (Encoding.UTF8.GetByteCount(jwtSettings.SigningKey) < 32)
        {
            throw new InvalidOperationException(
                "La clave de firma JWT debe contener al menos 32 bytes.");
        }

        if (jwtSettings.ExpirationMinutes <= 0)
        {
            throw new InvalidOperationException(
                "La duración del JWT debe ser mayor que cero.");
        }
    }
}
