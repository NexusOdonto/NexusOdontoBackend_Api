using Domain.Entities.Clinical;

namespace Application.Contracts.Repositories;

public interface IClinicalHistoryRepository : IRepository<ClinicalHistory>
{
    Task<ClinicalHistory?> GetByPatientIdAsync(
        Guid patientId,
        CancellationToken cancellationToken = default);
}
