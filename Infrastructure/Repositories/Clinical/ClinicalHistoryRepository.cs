using Application.Contracts.Repositories.Clinical;
using Domain.Entities.Clinical;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Clinical;

public sealed class ClinicalHistoryRepository : Common.Repository<ClinicalHistoryEntity>, IClinicalHistoryRepository
{
    public ClinicalHistoryRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<ClinicalHistoryEntity?> GetByPatientIdAsync(
        Guid patientId,
        CancellationToken cancellationToken = default)
    {
        return await Set.FirstOrDefaultAsync(h => h.PatientId == patientId, cancellationToken);
    }
}
