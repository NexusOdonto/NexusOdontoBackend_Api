using Application.Contracts.Repositories.Common;
using Domain.Entities.Clinical;

namespace Application.Contracts.Repositories.Clinical;

public interface IClinicalAttentionRepository : IRepository<ClinicalAttentionEntity>
{
    Task<ClinicalAttentionEntity?> GetByAppointmentIdAsync(
        Guid appointmentId,
        CancellationToken cancellationToken = default);
}
