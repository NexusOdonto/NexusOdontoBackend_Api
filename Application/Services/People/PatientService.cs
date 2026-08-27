using Application.Contracts.Repositories;
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

        var person = await personRepository.GetByIdAsync(request.PersonaId)
            ?? throw new InvalidOperationException($"Person {request.PersonaId} was not found.");

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var patient = new Patient(
                person.Id,
                request.ContactoEmergencia,
                request.TelefonoEmergencia);
            await patientRepository.AddAsync(patient);

            var history = new ClinicalHistory(patient.Id);
            await clinicalHistoryRepository.AddAsync(history);

            return Map(patient);
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

    private static object Map(Patient e) => new
    {
        e.Id,
        e.PersonId,
        e.EmergencyContact,
        e.EmergencyPhone,
        e.IsActive,
        e.CreatedAt
    };
}
