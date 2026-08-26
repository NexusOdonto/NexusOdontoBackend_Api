using Application.Contracts.Repositories;
using Application.DTOs;
using Domain.Entities.Clinical;
using Domain.Entities.People;

namespace Application.Services;

public sealed class PatientService(
    IRepository<Person> personRepository,
    IRepository<Patient> patientRepository,
    IRepository<ClinicalHistory> clinicalHistoryRepository,
    IUnitOfWork unitOfWork)
{
    public async Task<Guid> CreateAsync(
        CreatePatientRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        await unitOfWork.BeginTransactionAsync();

        try
        {
            var person = new Person(
                request.DocumentTypeId,
                request.DocumentNumber,
                request.FirstName,
                request.LastName,
                request.DateOfBirth,
                request.SexId,
                request.Phone,
                request.Email,
                request.Address);
            await personRepository.AddAsync(person);

            var patient = new Patient(
                person.Id,
                request.EmergencyContact,
                request.EmergencyPhone);
            await patientRepository.AddAsync(patient);

            var clinicalHistory = new ClinicalHistory(patient.Id);
            await clinicalHistoryRepository.AddAsync(clinicalHistory);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync();

            return patient.Id;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
