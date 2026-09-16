using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.People;
using Application.Contracts.Repositories.Security;
using Application.Contracts.Services;
using Application.DTOs.Auth;
using Application.Exceptions;
using Application.Services.People;
using Application.Services.Security;
using Domain.Entities.Audit;
using Domain.Entities.People;
using Domain.Entities.Security;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public sealed class AuthService(
    IUserRepository userRepository,
    IPersonRepository personRepository,
    IUserExternalLoginRepository externalLoginRepository,
    ISessionRepository sessionRepository,
    IRepository<AuditEventEntity> auditEventRepository,
    IUnitOfWork unitOfWork,
    IValidator<LoginRequestDto> loginValidator,
    IValidator<GoogleLinkRequestDto> googleLinkValidator,
    IValidator<RefreshTokenRequestDto> refreshTokenValidator,
    IValidator<UpdateProfileRequestDto> updateProfileValidator,
    JwtSettings jwtSettings,
    ChatbotSettings chatbotSettings) : IAuthService
{
    public const string GoogleProvider = "Google";
    public const string GoogleLinkTokenPurpose = "google-link";

    private static readonly Guid SignInEventTypeId =
        new("16000000-0000-0000-0000-000000000001");

    public async Task<AuthResponseDto?> LoginAsync(
        LoginRequestDto request,
        string? internalSecret = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var validationResult = await loginValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new Exceptions.ValidationException(validationResult.Errors);
        }

        var user = await ResolveUserByLoginIdAsync(request.LoginId, cancellationToken);

        if (!IsValidForPasswordSignIn(user, request.Password))
        {
            return null;
        }

        if (IsBotServiceUser(user!) && !HasValidInternalSecret(internalSecret))
        {
            return null;
        }

        return await SignInUserAsync(user!, cancellationToken);
    }

    public async Task<AuthResponseDto?> SignInByExternalLoginAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdWithRolesAsync(userId, cancellationToken);

        if (user is null ||
            !user.IsActive ||
            user.PersonEntity is null ||
            !user.PersonEntity.IsActive)
        {
            return null;
        }

        // Interactive Google / register auto-sign-in must never mint a BOT_SERVICE JWT.
        if (IsBotServiceUser(user))
        {
            return null;
        }

        return await SignInUserAsync(user, cancellationToken);
    }

    public async Task<AuthResponseDto?> LinkGoogleAsync(
        GoogleLinkRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var validationResult = await googleLinkValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new Exceptions.ValidationException(validationResult.Errors);
        }

        var linkClaims = ValidateGoogleLinkToken(request.LinkToken);
        if (linkClaims is null)
        {
            return null;
        }

        var existingLogin = await externalLoginRepository.GetByProviderKeyAsync(
            GoogleProvider,
            linkClaims.GoogleSub,
            cancellationToken);

        if (existingLogin is not null)
        {
            throw new ConflictException("This Google account is already linked to another user.");
        }

        var user = await ResolveUserByLoginIdAsync(request.LoginId, cancellationToken);

        if (!IsValidForPasswordSignIn(user, request.Password))
        {
            return null;
        }

        if (IsBotServiceUser(user!))
        {
            return null;
        }

        var userProviderLogin = await externalLoginRepository.GetByUserAndProviderAsync(
            user!.Id,
            GoogleProvider,
            cancellationToken);

        if (userProviderLogin is not null)
        {
            throw new ConflictException("This user already has a linked Google account.");
        }

        await externalLoginRepository.AddAsync(
            new UserExternalLoginEntity(
                user.Id,
                GoogleProvider,
                linkClaims.GoogleSub,
                linkClaims.GoogleEmail),
            cancellationToken);

        return await SignInUserAsync(user, cancellationToken);
    }

    public async Task UnlinkGoogleAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user is null || !user.IsActive)
        {
            throw new UnauthorizedAccessException("User is inactive or not found.");
        }

        var googleLogin = await externalLoginRepository.GetByUserAndProviderAsync(
            userId,
            GoogleProvider,
            cancellationToken);

        if (googleLogin is null)
        {
            throw new NotFoundException("This account does not have a linked Google account.");
        }

        if (!HasPasswordLogin(user))
        {
            throw new BusinessRuleException(
                "Define una contraseña antes de desvincular Google. Si no, la cuenta se quedaría sin forma de iniciar sesión.");
        }

        externalLoginRepository.Remove(googleLogin);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public string CreateGoogleLinkToken(string googleSub, string? googleEmail)
    {
        ValidateJwtSettings();

        var expiresAtUtc = DateTime.UtcNow.AddMinutes(5);
        var claims = new List<Claim>
        {
            new("purpose", GoogleLinkTokenPurpose),
            new("googleSub", googleSub)
        };

        if (!string.IsNullOrWhiteSpace(googleEmail))
        {
            claims.Add(new Claim("googleEmail", googleEmail.Trim().ToLowerInvariant()));
        }

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: expiresAtUtc,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public GoogleLinkTokenClaims? ValidateGoogleLinkToken(string linkToken)
    {
        if (string.IsNullOrWhiteSpace(linkToken))
        {
            return null;
        }

        ValidateJwtSettings();

        var tokenHandler = new JwtSecurityTokenHandler();
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey));

        try
        {
            var principal = tokenHandler.ValidateToken(
                linkToken,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = signingKey,
                    ClockSkew = TimeSpan.FromMinutes(1)
                },
                out _);

            var purpose = principal.FindFirstValue("purpose");
            var googleSub = principal.FindFirstValue("googleSub");

            if (!string.Equals(purpose, GoogleLinkTokenPurpose, StringComparison.Ordinal) ||
                string.IsNullOrWhiteSpace(googleSub))
            {
                return null;
            }

            return new GoogleLinkTokenClaims(
                googleSub,
                principal.FindFirstValue("googleEmail"));
        }
        catch
        {
            return null;
        }
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

        if (user.PersonEntity is null || !user.PersonEntity.IsActive)
        {
            return null;
        }

        return await MapProfileAsync(user, cancellationToken);
    }

    public async Task<AuthProfileDto?> UpdateProfileAsync(
        Guid userId,
        UpdateProfileRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var validationResult = await updateProfileValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new Exceptions.ValidationException(validationResult.Errors);
        }

        var user = await userRepository.GetByIdWithRolesAsync(userId, cancellationToken);
        if (user is null || !user.IsActive)
        {
            return null;
        }

        var person = user.PersonEntity;
        if (person is null || !person.IsActive)
        {
            return null;
        }

        var documentNumber = string.IsNullOrWhiteSpace(request.DocumentNumber)
            ? person.DocumentNumber
            : request.DocumentNumber.Trim();

        if (!string.Equals(documentNumber, person.DocumentNumber, StringComparison.OrdinalIgnoreCase))
        {
            var existingPerson = await personRepository.GetByDocumentNumberAsync(documentNumber);
            if (existingPerson is not null && existingPerson.Id != person.Id)
            {
                throw new ConflictException("A person with this document number already exists.");
            }
        }

        person.DocumentNumber = documentNumber;
        person.Update(
            request.FirstName.Trim(),
            request.LastName.Trim(),
            person.DateOfBirth,
            person.SexId,
            string.IsNullOrWhiteSpace(request.Phone) ? person.Phone : request.Phone.Trim(),
            string.IsNullOrWhiteSpace(request.Email) ? person.Email : request.Email.Trim(),
            string.IsNullOrWhiteSpace(request.Address) ? person.Address : request.Address.Trim());

        personRepository.Update(person);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return await MapProfileAsync(user, cancellationToken);
    }

    public async Task<bool> ChangePasswordAsync(
        Guid userId,
        string newPassword,
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user is null || !user.IsActive)
        {
            return false;
        }

        var newHash = BCrypt.Net.BCrypt.HashPassword(newPassword, 11);
        user.PasswordHash = newHash;
        user.UpdatedAt = DateTime.UtcNow;

        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    private async Task<AuthResponseDto> SignInUserAsync(
        UserEntity user,
        CancellationToken cancellationToken)
    {
        user.RegisterAccess(DateTime.UtcNow);
        userRepository.Update(user);

        var auditEvent = new AuditEventEntity(
            SignInEventTypeId,
            "User signed in.",
            user.Id);
        await auditEventRepository.AddAsync(auditEvent);

        var response = BuildAccessTokenResponse(user);
        await AttachNewSessionAsync(user.Id, response, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return response;
    }

    private async Task<UserEntity?> ResolveUserByLoginIdAsync(
        string loginId,
        CancellationToken cancellationToken)
    {
        var normalized = loginId.Trim();

        if (EmployeeCodeGenerator.LooksLikeEmployeeCode(normalized))
        {
            return await userRepository.GetByEmployeeCodeWithRolesAsync(
                EmployeeCodeGenerator.NormalizeEmployeeCode(normalized),
                cancellationToken);
        }

        return await userRepository.GetByDocumentNumberWithRolesAsync(
            normalized,
            cancellationToken);
    }

    private static bool HasPasswordLogin(UserEntity user)
    {
        var hash = user.PasswordHash;
        return !string.IsNullOrWhiteSpace(hash) &&
               (hash.StartsWith("$2a$", StringComparison.Ordinal) ||
                hash.StartsWith("$2b$", StringComparison.Ordinal) ||
                hash.StartsWith("$2y$", StringComparison.Ordinal));
    }

    private static bool IsValidForPasswordSignIn(UserEntity? user, string password)
    {
        return user is not null &&
               user.IsActive &&
               user.PersonEntity is not null &&
               user.PersonEntity.IsActive &&
               BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }

    private static bool IsBotServiceUser(UserEntity user)
    {
        return user.UserRoles.Any(ur =>
            ur.RoleId == ReservedServiceAccounts.BotServiceRoleId ||
            string.Equals(ur.RoleEntity?.Code, "BOT_SERVICE", StringComparison.OrdinalIgnoreCase));
    }

    private bool HasValidInternalSecret(string? providedSecret)
    {
        var expected = chatbotSettings.InternalSecret;
        if (string.IsNullOrWhiteSpace(expected) || string.IsNullOrWhiteSpace(providedSecret))
        {
            return false;
        }

        return string.Equals(expected.Trim(), providedSecret.Trim(), StringComparison.Ordinal);
    }

    private async Task AttachNewSessionAsync(
        Guid userId,
        AuthResponseDto response,
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
            SessionStatusIds.Active);

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

    private async Task<AuthProfileDto> MapProfileAsync(
        UserEntity user,
        CancellationToken cancellationToken)
    {
        var googleLogin = await externalLoginRepository.GetByUserAndProviderAsync(
            user.Id,
            GoogleProvider,
            cancellationToken);

        return new AuthProfileDto
        {
            User = MapToResponse(user),
            Permissions = PermissionChecker.GetEffectivePermissions(user),
            GoogleLinked = googleLogin is not null,
            GoogleEmail = googleLogin?.ProviderEmail
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
            FirstName = person?.FirstName ?? string.Empty,
            LastName = person?.LastName ?? string.Empty,
            DocumentNumber = person?.DocumentNumber ?? string.Empty,
            Phone = person?.Phone,
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
