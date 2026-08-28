using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.People;
using Application.Contracts.Repositories.Clinical;
using Application.Contracts.Services.People;
using Application.DTOs.Pacientes;
using Domain.Entities.Clinical;
using Domain.Entities.People;

namespace Application.Services.People;

public sealed class PatientService(
    IPatientRepository patientRepository,
    IPersonRepository personRepository,
    IClinicalHistoryRepository clinicalHistoryRepository,
    IUnitOfWork unitOfWork) : IPatientService
{
    public async Task<IReadOnlyList<object>> GetAllAsync()
    {
        var items = await patientRepository.GetAllAsync();
        return items.Select(Map).Cast<object>().ToList();
    }

    public async Task<object?> GetByIdAsync(Guid id)
    {
        var entity = await patientRepository.GetByIdWithPersonAsync(id)
            ?? await patientRepository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<object> CreateAsync(CreatePacienteDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var PersonEntity = await personRepository.GetByIdAsync(request.PersonaId)
            ?? throw new InvalidOperationException($"PersonEntity {request.PersonaId} was not found.");

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var PatientEntity = new PatientEntity(
                PersonEntity.Id,
                request.ContactoEmergencia,
                request.TelefonoEmergencia);
            await patientRepository.AddAsync(PatientEntity);

            var history = new ClinicalHistoryEntity(PatientEntity.Id);
            await clinicalHistoryRepository.AddAsync(history);

            return Map(PatientEntity);
        });
    }

    public async Task<object?> UpdateAsync(Guid id, UpdatePacienteDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await patientRepository.GetByIdAsync(id);
        if (entity is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.Update(request.ContactoEmergencia, request.TelefonoEmergencia);
            entity.IsActive = request.Activo;
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

    private static object Map(PatientEntity e) => new
    {
        e.Id,
        e.PersonId,
        e.EmergencyContact,
        e.EmergencyPhone,
        e.IsActive,
        e.CreatedAt
    };
}
