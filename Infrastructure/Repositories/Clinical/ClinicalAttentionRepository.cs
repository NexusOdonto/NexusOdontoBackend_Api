using Application.Contracts.Repositories.Clinical;
using Domain.Entities.Clinical;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Clinical;

public sealed class ClinicalAttentionRepository : Common.Repository<ClinicalAttentionEntity>, IClinicalAttentionRepository
{
    public ClinicalAttentionRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<ClinicalAttentionEntity?> GetByAppointmentIdAsync(
        Guid appointmentId,
        CancellationToken cancellationToken = default)
    {
        return await Set.FirstOrDefaultAsync(a => a.AppointmentId == appointmentId, cancellationToken);
    }
}
