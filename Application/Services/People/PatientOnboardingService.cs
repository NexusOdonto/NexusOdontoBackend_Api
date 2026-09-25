using Application.Contracts.Repositories.Clinical;
using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.People;
using Application.Contracts.Services.People;
using Application.DTOs.Patients;
using Application.Exceptions;
using Application.Services.Security;
using Domain.Entities.Clinical;
using Domain.Entities.People;
using Domain.Entities.Security;
using FluentValidation;
using System.Globalization;
using System.Text;

namespace Application.Services.People;

public sealed class PatientOnboardingService(
    IPersonRepository personRepository,
    IPatientRepository patientRepository,
    IUserRepository userRepository,
    IClinicalHistoryRepository clinicalHistoryRepository,
    IUnitOfWork unitOfWork,
    IValidator<OnboardPatientDto> validator) : IPatientOnboardingService
{
    private static readonly Guid PatientRoleId = new("13000000-0000-0000-0000-000000000005");

    private static readonly string[] PlaceholderNameTokens =
    [
        "paciente", "nexus", "nexusodonto", "desconocido", "anonimo", "anónimo", "n/a", "none", "usuario", "cliente"
    ];

    public async Task<OnboardPatientResponseDto> OnboardAsync(
        OnboardPatientDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new Exceptions.ValidationException(validationResult.Errors);
        }

        if (ReservedServiceAccounts.IsReservedIdentifier(request.DocumentNumber) ||
            ReservedServiceAccounts.IsReservedIdentifier(request.Email))
        {
            throw new BusinessRuleException(
                "This document number or email is reserved for a system service account and cannot be registered.");
        }

        var documentNumber = request.DocumentNumber.Trim();

        // Cédula is the unique identity key: never create a second person for the same document.
        // Include inactive so soft-deleted identities are reactivated instead of blocked/duplicated.
        var existingPerson = await personRepository.GetByDocumentNumberIncludingInactiveAsync(
            documentNumber,
            cancellationToken);

        if (existingPerson is not null)
        {
            return await ServiceWrite.InTransactionAsync(
                unitOfWork,
                () => EnsurePatientAndUserForPersonAsync(existingPerson, request, alreadyExisted: true, cancellationToken));
        }

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            // Race: another request may have created the person between the check and now.
            var racedPerson = await personRepository.GetByDocumentNumberIncludingInactiveAsync(
                documentNumber,
                cancellationToken);
            if (racedPerson is not null)
            {
                return await EnsurePatientAndUserForPersonAsync(
                    racedPerson, request, alreadyExisted: true, cancellationToken);
            }

            var emergencyContact = NormalizeEmergencyContact(request.EmergencyContact);
            var emergencyPhone = NormalizeEmergencyPhone(request.EmergencyPhone);

            var person = new PersonEntity(
                request.DocumentTypeId,
                documentNumber,
                request.FirstName,
                request.LastName,
                request.DateOfBirth,
                request.SexId,
                request.Phone,
                request.Email,
                request.Address);
            await personRepository.AddAsync(person);

            var patient = new PatientEntity(person.Id, emergencyContact, emergencyPhone);
            await patientRepository.AddAsync(patient);

            var history = new ClinicalHistoryEntity(patient.Id);
            await clinicalHistoryRepository.AddAsync(history);

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new UserEntity(person.Id, passwordHash);
            user.UserRoles.Add(new UserRoleEntity(user.Id, PatientRoleId));
            await userRepository.AddAsync(user);

            return new OnboardPatientResponseDto
            {
                PersonId = person.Id,
                PatientId = patient.Id,
                UserId = user.Id,
                RoleId = PatientRoleId,
                AlreadyExisted = false
            };
        });
    }

    private async Task<OnboardPatientResponseDto> EnsurePatientAndUserForPersonAsync(
        PersonEntity existingPerson,
        OnboardPatientDto request,
        bool alreadyExisted,
        CancellationToken cancellationToken)
    {
        // Chatbot/portal booking must succeed for returning soft-deleted patients:
        // reactivate person + clinical row + login instead of failing with a technical error.
        if (!existingPerson.IsActive)
        {
            existingPerson.SetActive(true);
            personRepository.Update(existingPerson);
        }

        MaybeRefreshDisplayName(existingPerson, request);
        MaybeRefreshPhone(existingPerson, request);

        var patient = await patientRepository.GetByPersonIdAsync(existingPerson.Id, cancellationToken);
        if (patient is null)
        {
            patient = new PatientEntity(
                existingPerson.Id,
                NormalizeEmergencyContact(request.EmergencyContact),
                NormalizeEmergencyPhone(request.EmergencyPhone));
            await patientRepository.AddAsync(patient);

            var history = new ClinicalHistoryEntity(patient.Id);
            await clinicalHistoryRepository.AddAsync(history);
        }
        else if (!patient.IsActive)
        {
            patient.SetActive(true);
            patientRepository.Update(patient);
        }

        var user = await userRepository.GetByPersonIdAsync(existingPerson.Id, cancellationToken);
        if (user is null)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user = new UserEntity(existingPerson.Id, passwordHash);
            user.UserRoles.Add(new UserRoleEntity(user.Id, PatientRoleId));
            await userRepository.AddAsync(user);
        }
        else if (!user.IsActive)
        {
            user.SetActive(true);
            userRepository.Update(user);
        }

        return new OnboardPatientResponseDto
        {
            PersonId = existingPerson.Id,
            PatientId = patient.Id,
            UserId = user.Id,
            RoleId = PatientRoleId,
            AlreadyExisted = alreadyExisted
        };
    }

    private void MaybeRefreshDisplayName(PersonEntity person, OnboardPatientDto request)
    {
        var current = $"{person.FirstName} {person.LastName}".Trim();
        var incomingFirst = (request.FirstName ?? string.Empty).Trim();
        var incomingLast = (request.LastName ?? string.Empty).Trim();
        var incoming = $"{incomingFirst} {incomingLast}".Trim();

        if (string.IsNullOrWhiteSpace(incoming) || IsPlaceholderName(incoming))
            return;

        // Keep canonical registered name when the typed name is a token-subset
        // (e.g. "Alejandro Escobar" ⊆ "Jhon Alejandro Escobar Lozada").
        if (!IsPlaceholderName(current) && IsNameTokenSubset(incoming, current))
            return;

        // Prefer a longer/more complete real name when the stored one is placeholder,
        // or when the registered name is itself a subset of a richer incoming name.
        var shouldRefresh =
            IsPlaceholderName(current) ||
            (IsNameTokenSubset(current, incoming) &&
             incoming.Length > current.Length + 2 &&
             !string.Equals(current, incoming, StringComparison.OrdinalIgnoreCase)) ||
            (incoming.Length > current.Length + 2 &&
             NamesShareSignificantToken(current, incoming) &&
             !string.Equals(current, incoming, StringComparison.OrdinalIgnoreCase));

        if (!shouldRefresh)
            return;

        person.Update(
            incomingFirst,
            string.IsNullOrWhiteSpace(incomingLast) ? person.LastName : incomingLast,
            person.DateOfBirth,
            person.SexId,
            person.Phone,
            person.Email,
            person.Address);
        personRepository.Update(person);
    }

    private static readonly HashSet<string> PlaceholderPhoneDigits = new(StringComparer.Ordinal)
    {
        "3000000000",
        "0000000000",
        "1111111111",
        "1234567890",
        "3001234567",
        "573000000000",
        "573001234567",
        "573002345678",
    };

    private void MaybeRefreshPhone(PersonEntity person, OnboardPatientDto request)
    {
        var incoming = NormalizeWhatsAppPhone(request.Phone);
        if (string.IsNullOrWhiteSpace(incoming))
            return;

        if (!IsPlaceholderPhone(person.Phone) && !string.IsNullOrWhiteSpace(person.Phone))
            return; // keep user-edited / already-real phone

        if (string.Equals(DigitsOnly(person.Phone), DigitsOnly(incoming), StringComparison.Ordinal))
            return;

        person.Update(
            person.FirstName,
            person.LastName,
            person.DateOfBirth,
            person.SexId,
            incoming,
            person.Email,
            person.Address);
        personRepository.Update(person);
    }

    private static string? NormalizeWhatsAppPhone(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return null;

        var trimmed = raw.Trim();
        if (trimmed.Contains("@lid", StringComparison.OrdinalIgnoreCase))
            return null;

        var userPart = trimmed.Contains('@') ? trimmed.Split('@')[0] : trimmed;
        var digits = DigitsOnly(userPart);
        if (string.IsNullOrEmpty(digits) || IsPlaceholderPhone(digits))
            return null;

        // WhatsApp LID opaque ids are typically 14+ digits — never treat as E.164
        if (digits.Length >= 14)
            return null;

        if (digits.Length == 10 && digits.StartsWith('3'))
            return "+57" + digits;
        if (digits.Length == 12 && digits.StartsWith("573", StringComparison.Ordinal))
            return "+" + digits;
        if (digits.Length is >= 10 and <= 13)
            return "+" + digits;

        return null;
    }

    private static bool IsPlaceholderPhone(string? phone)
    {
        var digits = DigitsOnly(phone);
        if (string.IsNullOrEmpty(digits))
            return true;
        if (PlaceholderPhoneDigits.Contains(digits))
            return true;
        var local = digits.StartsWith("57", StringComparison.Ordinal) && digits.Length > 2
            ? digits[2..]
            : digits;
        return PlaceholderPhoneDigits.Contains(local);
    }

    private static string DigitsOnly(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;
        return new string(value.Where(char.IsDigit).ToArray());
    }

    private static bool IsPlaceholderName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName) || fullName.Length < 3)
            return true;

        var lower = fullName.ToLowerInvariant();
        return PlaceholderNameTokens.Any(t => lower.Contains(t, StringComparison.Ordinal));
    }

    private static readonly HashSet<string> NameTitleTokens = new(StringComparer.OrdinalIgnoreCase)
    {
        "dr", "dra", "doctor", "doctora", "sr", "sra", "srta", "don", "dona", "doña", "mr", "mrs", "ms"
    };

    private static readonly HashSet<string> NameStopwords = new(StringComparer.OrdinalIgnoreCase)
    {
        "de", "del", "la", "las", "los", "y", "e", "da", "do", "dos"
    };

    private static IReadOnlyList<string> SignificantNameTokens(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return Array.Empty<string>();

        var normalized = fullName.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(normalized.Length);
        foreach (var c in normalized)
        {
            var cat = CharUnicodeInfo.GetUnicodeCategory(c);
            if (cat == UnicodeCategory.NonSpacingMark)
                continue;
            sb.Append(char.IsLetterOrDigit(c) ? c : ' ');
        }

        return sb.ToString()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(t => t.Length >= 2 && !NameTitleTokens.Contains(t) && !NameStopwords.Contains(t))
            .ToArray();
    }

    /// <summary>
    /// True when every significant token of <paramref name="needle"/> appears in
    /// <paramref name="haystack"/> (order-independent containment).
    /// </summary>
    private static bool IsNameTokenSubset(string needle, string haystack)
    {
        var needleTokens = SignificantNameTokens(needle);
        var haystackTokens = SignificantNameTokens(haystack);
        if (needleTokens.Count == 0 || haystackTokens.Count == 0)
            return false;

        var hay = haystackTokens.ToHashSet(StringComparer.OrdinalIgnoreCase);
        return needleTokens.All(t => hay.Contains(t));
    }

    private static bool NamesShareSignificantToken(string a, string b)
    {
        var setA = SignificantNameTokens(a).ToHashSet(StringComparer.OrdinalIgnoreCase);
        if (setA.Count == 0)
            return false;
        return SignificantNameTokens(b).Any(t => setA.Contains(t));
    }

    private static string NormalizeEmergencyContact(string? value) =>
        string.IsNullOrWhiteSpace(value) ? "Recepción Nexus" : value.Trim();

    private static string NormalizeEmergencyPhone(string? value) =>
        string.IsNullOrWhiteSpace(value) ? "+573246030217" : value.Trim();
}
