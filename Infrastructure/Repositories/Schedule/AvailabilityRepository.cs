using Application.Contracts.Repositories.Schedule;
using Domain.Entities.Schedule;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Schedule;

public sealed class AvailabilityRepository : Common.Repository<AvailabilityEntity>, IAvailabilityRepository
{
    public AvailabilityRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<AvailabilityEntity>> GetByProfessionalAsync(
        Guid professionalId,
        CancellationToken cancellationToken = default)
    {
        return await Set
            .Where(a => a.ProfessionalId == professionalId)
            .ToListAsync(cancellationToken);
    }
}
