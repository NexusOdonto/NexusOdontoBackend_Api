using Domain.Entities.Clinical;

namespace Application.Contracts.Repositories;

public interface IClinicalAttentionRepository : IRepository<ClinicalAttention>
{
    Task<ClinicalAttention?> GetByAppointmentIdAsync(
        Guid appointmentId,
        CancellationToken cancellationToken = default);
}
