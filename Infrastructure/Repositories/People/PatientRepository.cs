using Application.Contracts.Repositories.People;
using Domain.Entities.People;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.People;

public sealed class PatientRepository : Common.Repository<PatientEntity>, IPatientRepository
{
    public PatientRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<PatientEntity?> GetByPersonIdAsync(
        Guid personId,
        CancellationToken cancellationToken = default)
    {
        return await Set.FirstOrDefaultAsync(p => p.PersonId == personId, cancellationToken);
    }

    public async Task<PatientEntity?> GetByIdWithPersonAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await Set
            .Include(p => p.PersonEntity)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}
