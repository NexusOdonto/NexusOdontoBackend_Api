using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;

using System.Text;

using Application.Contracts.Repositories.Common;

using Application.Contracts.Repositories.People;

using Application.Contracts.Services;

using Application.DTOs.Auth;

using Application.Exceptions;

using Application.Services.Security;

using Domain.Entities.Audit;

using Domain.Entities.People;

using Domain.Entities.Security;

using FluentValidation;

using Microsoft.IdentityModel.Tokens;



namespace Application.Services;



public sealed class AuthService(

    IUserRepository userRepository,

    ISessionRepository sessionRepository,

    IRepository<AuditEventEntity> auditEventRepository,

    IUnitOfWork unitOfWork,

    IValidator<LoginRequestDto> loginValidator,

    IValidator<RefreshTokenRequestDto> refreshTokenValidator,

    JwtSettings jwtSettings) : IAuthService

{

    private static readonly Guid SignInEventTypeId =

        new("16000000-0000-0000-0000-000000000001");



    public async Task<AuthResponseDto?> LoginAsync(

        LoginRequestDto request,

        string? ipAddress = null,

        string? userAgent = null,

        CancellationToken cancellationToken = default)

    {

        ArgumentNullException.ThrowIfNull(request);



        var validationResult = await loginValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)

        {

            throw new Exceptions.ValidationException(validationResult.Errors);

        }



        string? email;

        if (request.Login.Contains('@'))

        {

            email = request.Login.Trim().ToLowerInvariant();

        }

        else

        {

            email = await userRepository.GetEmailByDocumentNumberAsync(

                request.Login.Trim(),

                cancellationToken);



            if (string.IsNullOrWhiteSpace(email))

            {

                return null;

            }



            email = email.Trim().ToLowerInvariant();

        }



        var user = await userRepository.GetByEmailWithRolesAsync(

            email,

            cancellationToken);



        if (user is null ||

            !user.IsActive ||

            user.PersonEntity is null ||

            !user.PersonEntity.IsActive ||

            string.IsNullOrWhiteSpace(user.PersonEntity.Email) ||

            !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))

        {

            return null;

        }



        user.RegisterAccess(DateTime.UtcNow);

        userRepository.Update(user);



        var auditEvent = new AuditEventEntity(

            SignInEventTypeId,

            "User signed in.",

            user.Id,

            ipAddress: ipAddress,

            userAgent: userAgent);

        await auditEventRepository.AddAsync(auditEvent);



        var response = BuildAccessTokenResponse(user);

        await AttachNewSessionAsync(

            user.Id,

            response,

            ipAddress,

            userAgent,

            cancellationToken);



        await unitOfWork.SaveChangesAsync(cancellationToken);



        return response;

    }



    public async Task<AuthResponseDto?> RefreshAsync(

        RefreshTokenRequestDto request,

        CancellationToken cancellationToken = default)

    {

        ArgumentNullException.ThrowIfNull(request);



        var validationResult = await refreshTokenValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)

        {

            throw new Exceptions.ValidationException(validationResult.Errors);

        }



        var refreshHash = RefreshTokenHelper.HashToken(request.RefreshToken);

        var session = await sessionRepository.GetByRefreshTokenHashAsync(

            refreshHash,

            cancellationToken);



        if (session is null || !IsSessionActive(session))

        {

            return null;

        }



        var user = await userRepository.GetByIdWithRolesAsync(session.UserId, cancellationToken);

        if (user is null ||

            !user.IsActive ||

            user.PersonEntity is null ||

            !user.PersonEntity.IsActive)

        {

            return null;

        }



        var newRefreshToken = RefreshTokenHelper.GenerateToken();

        session.RefreshTokenHash = RefreshTokenHelper.HashToken(newRefreshToken);

        session.LastUsedAt = DateTime.UtcNow;

        sessionRepository.Update(session);



        var response = BuildAccessTokenResponse(user);

        response.RefreshToken = newRefreshToken;

        response.RefreshExpiresAtUtc = session.ExpiresAt;



        await unitOfWork.SaveChangesAsync(cancellationToken);



        return response;

    }



    public async Task<bool> LogoutAsync(

        RefreshTokenRequestDto request,

        CancellationToken cancellationToken = default)

    {

        ArgumentNullException.ThrowIfNull(request);



        var validationResult = await refreshTokenValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)

        {

            throw new Exceptions.ValidationException(validationResult.Errors);

        }



        var refreshHash = RefreshTokenHelper.HashToken(request.RefreshToken);

        var session = await sessionRepository.GetByRefreshTokenHashAsync(

            refreshHash,

            cancellationToken);



        if (session is null || session.RevokedAt is not null)

        {

            return false;

        }



        session.Update(SessionStatusIds.Revoked, session.LastUsedAt, DateTime.UtcNow);

        sessionRepository.Update(session);

        await unitOfWork.SaveChangesAsync(cancellationToken);



        return true;

    }



    public async Task<AuthProfileDto?> GetProfileAsync(

        Guid userId,

        CancellationToken cancellationToken = default)

    {

        var user = await userRepository.GetByIdWithRolesAsync(userId, cancellationToken);

        if (user is null || !user.IsActive)

        {

            return null;

        }



        if (user.PersonEntity is null ||

            !user.PersonEntity.IsActive ||

            string.IsNullOrWhiteSpace(user.PersonEntity.Email))

        {

            return null;

        }



        return new AuthProfileDto

        {

            User = MapToResponse(user),

            Permissions = PermissionChecker.GetEffectivePermissions(user)

        };

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



    private async Task AttachNewSessionAsync(

        Guid userId,

        AuthResponseDto response,

        string? ipAddress,

        string? userAgent,

        CancellationToken cancellationToken)

    {

        ValidateJwtSettings();



        var refreshToken = RefreshTokenHelper.GenerateToken();

        var refreshExpiresAtUtc = DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenExpirationDays);



        var session = new SessionEntity(

            userId,

            RefreshTokenHelper.HashToken(refreshToken),

            Guid.NewGuid().ToString("N"),

            refreshExpiresAtUtc,

            SessionStatusIds.Active,

            ipAddress: ipAddress,

            userAgent: userAgent);



        await sessionRepository.AddAsync(session);



        response.RefreshToken = refreshToken;

        response.RefreshExpiresAtUtc = refreshExpiresAtUtc;

    }



    private static bool IsSessionActive(SessionEntity session)

    {

        return session.RevokedAt is null &&

               session.ExpiresAt > DateTime.UtcNow &&

               session.SessionStatusId == SessionStatusIds.Active;

    }



    private AuthResponseDto BuildAccessTokenResponse(UserEntity user)

    {

        ValidateJwtSettings();



        var expiresAtUtc = DateTime.UtcNow.AddMinutes(jwtSettings.ExpirationMinutes);

        var claims = new List<Claim>

        {

            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),

            new(ClaimTypes.NameIdentifier, user.Id.ToString())

        };



        foreach (var userRole in user.UserRoles)

        {

            if (userRole.RoleEntity is null)

            {

                continue;

            }



            claims.Add(new Claim("roleId", userRole.RoleId.ToString()));

            claims.Add(new Claim(ClaimTypes.Role, userRole.RoleEntity.Name));

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

            ExpiresAtUtc = expiresAtUtc,

            User = MapToResponse(user),

            Permissions = PermissionChecker.GetEffectivePermissions(user)

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



        if (jwtSettings.RefreshTokenExpirationDays <= 0)

        {

            throw new InvalidOperationException(

                "Refresh token expiration must be greater than zero.");

        }

    }

}


