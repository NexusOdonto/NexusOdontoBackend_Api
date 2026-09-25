using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.People;
using Application.Contracts.Repositories.Clinical;
using Application.Contracts.Services.People;
using Application.DTOs.Patients;
using Application.Exceptions;
using Domain.Entities.Clinical;
using Domain.Entities.People;

namespace Application.Services.People;

public sealed class PatientService(
    IPatientRepository patientRepository,
    IPersonRepository personRepository,
    IClinicalHistoryRepository clinicalHistoryRepository,
    IUnitOfWork unitOfWork) : IPatientService
{
    public async Task<IReadOnlyList<PatientResponseDto>> GetAllAsync()
    {
        var items = await patientRepository.GetAllAsync();
        var result = new List<PatientResponseDto>(items.Count);
        foreach (var patient in items)
        {
            if (!patient.IsActive)
                continue;

            // GetById ignores soft-delete filters so we can detect orphaned active patients.
            var person = await personRepository.GetByIdAsync(patient.PersonId);
            if (person is null || !person.IsActive)
                continue;

            result.Add(Map(patient));
        }

        return result;
    }

    public async Task<PatientResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await patientRepository.GetByIdWithPersonAsync(id)
            ?? await patientRepository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<PatientResponseDto?> GetByPersonIdAsync(
        Guid personId,
        CancellationToken cancellationToken = default)
    {
        var entity = await patientRepository.GetByPersonIdAsync(personId, cancellationToken);
        return entity is null ? null : Map(entity);
    }

    public async Task<PatientResponseDto?> GetByDocumentNumberAsync(
        string documentNumber,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(documentNumber))
            return null;

        var person = await personRepository.GetByDocumentNumberAsync(
            documentNumber.Trim(),
            cancellationToken);
        if (person is null)
            return null;

        var patient = await patientRepository.GetByPersonIdAsync(person.Id, cancellationToken);
        return patient is null ? null : Map(patient);
    }

    public async Task<PatientResponseDto> CreateAsync(CreatePatientDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var person = await personRepository.GetByIdAsync(request.PersonId)
            ?? throw new BusinessRuleException($"Person {request.PersonId} was not found.");

        // One patient per person — never create a second clinical record for the same identity.
        var existing = await patientRepository.GetByPersonIdAsync(person.Id);
        if (existing is not null)
            return Map(existing);

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            // Re-check inside the transaction to avoid races.
            var raced = await patientRepository.GetByPersonIdAsync(person.Id);
            if (raced is not null)
                return Map(raced);

            var patient = new PatientEntity(
                person.Id,
                request.EmergencyContact,
                request.EmergencyPhone);
            await patientRepository.AddAsync(patient);

            var history = new ClinicalHistoryEntity(patient.Id);
            await clinicalHistoryRepository.AddAsync(history);

            return Map(patient);
        });
    }

    public async Task<PatientResponseDto?> UpdateAsync(Guid id, UpdatePatientDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await patientRepository.GetByIdAsync(id);
        if (entity is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.Update(request.EmergencyContact, request.EmergencyPhone);
            entity.IsActive = request.IsActive;
            patientRepository.Update(entity);
            return Map(entity);
        });
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await patientRepository.GetByIdAsync(id);
        if (entity is null) return false;

        await ServiceWrite.InTransactionAsync(unitOfWork, () =>
        {
            entity.Deactivate();
            patientRepository.Update(entity);
            return Task.CompletedTask;
        });
        return true;
    }

    private static PatientResponseDto Map(PatientEntity e) => new()
    {
        Id = e.Id,
        PersonId = e.PersonId,
        EmergencyContact = e.EmergencyContact,
        EmergencyPhone = e.EmergencyPhone,
        IsActive = e.IsActive,
        CreatedAt = e.CreatedAt
    };
}
