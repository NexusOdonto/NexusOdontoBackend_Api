using Application.Contracts.Repositories.Clinical;
using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.People;
using Application.Contracts.Services.People;
using Application.DTOs.Patients;
using Application.Exceptions;
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

        var existingPerson = await personRepository.GetByDocumentAsync(
            request.DocumentTypeId,
            request.DocumentNumber.Trim(),
            cancellationToken);

        if (existingPerson is not null)
        {
            throw new ConflictException("A person with this document number already exists.");
        }

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var person = new PersonEntity(
                request.DocumentTypeId,
                request.DocumentNumber.Trim(),
                request.FirstName,
                request.LastName,
                request.DateOfBirth,
                request.SexId,
                request.Phone,
                request.Email,
                request.Address);
            await personRepository.AddAsync(person);

            var patient = new PatientEntity(
                person.Id,
                request.EmergencyContact,
                request.EmergencyPhone);
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
                RoleId = PatientRoleId
            };
        });
    }
}
