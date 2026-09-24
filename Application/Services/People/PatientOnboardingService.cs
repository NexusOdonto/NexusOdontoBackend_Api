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
        var existingPerson = await personRepository.GetByDocumentNumberAsync(
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
            var racedPerson = await personRepository.GetByDocumentNumberAsync(
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
        MaybeRefreshDisplayName(existingPerson, request);

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

        var user = await userRepository.GetByPersonIdAsync(existingPerson.Id, cancellationToken);
        if (user is null)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user = new UserEntity(existingPerson.Id, passwordHash);
            user.UserRoles.Add(new UserRoleEntity(user.Id, PatientRoleId));
            await userRepository.AddAsync(user);
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

        // Prefer a longer/more complete real name when the stored one is placeholder or shorter.
        if (IsPlaceholderName(current) ||
            (incoming.Length > current.Length + 2 &&
             !string.Equals(current, incoming, StringComparison.OrdinalIgnoreCase)))
        {
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
    }

    private static bool IsPlaceholderName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName) || fullName.Length < 3)
            return true;

        var lower = fullName.ToLowerInvariant();
        return PlaceholderNameTokens.Any(t => lower.Contains(t, StringComparison.Ordinal));
    }

    private static string NormalizeEmergencyContact(string? value) =>
        string.IsNullOrWhiteSpace(value) ? "Recepción Nexus" : value.Trim();

    private static string NormalizeEmergencyPhone(string? value) =>
        string.IsNullOrWhiteSpace(value) ? "+573246030217" : value.Trim();
}
