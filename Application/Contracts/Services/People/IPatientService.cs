using Application.Contracts.Services.Common;
using Application.DTOs.Patients;

namespace Application.Contracts.Services.People
{
    public interface IPatientService : ICrudService<PatientResponseDto, CreatePatientDto, UpdatePatientDto>
    {
        /// <summary>
        /// Resolves the canonical patient for a document number (cédula).
        /// </summary>
        Task<PatientResponseDto?> GetByDocumentNumberAsync(
            string documentNumber,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Resolves patient by person id without listing all patients.
        /// </summary>
        Task<PatientResponseDto?> GetByPersonIdAsync(
            Guid personId,
            CancellationToken cancellationToken = default);
    }
}
