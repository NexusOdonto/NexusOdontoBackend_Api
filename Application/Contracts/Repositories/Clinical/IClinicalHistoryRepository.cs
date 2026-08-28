using Application.Contracts.Repositories.Common;
using Domain.Entities.Clinical;

namespace Application.Contracts.Repositories.Clinical;

public interface IClinicalHistoryRepository : IRepository<ClinicalHistoryEntity>
{
    Task<ClinicalHistoryEntity?> GetByPatientIdAsync(
        Guid patientId,
        CancellationToken cancellationToken = default);
}
