using Application.Contracts.Repositories.Common;
using Domain.Entities.Schedule;

namespace Application.Contracts.Repositories.Schedule;

public interface IAvailabilityRepository : IRepository<AvailabilityEntity>
{
    Task<IReadOnlyList<AvailabilityEntity>> GetByProfessionalAsync(
        Guid professionalId,
        CancellationToken cancellationToken = default);
}
