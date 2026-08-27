using Domain.Entities.Schedule;

namespace Application.Contracts.Repositories;

public interface IAvailabilityRepository : IRepository<Availability>
{
    Task<IReadOnlyList<Availability>> GetByProfessionalAsync(
        Guid professionalId,
        CancellationToken cancellationToken = default);
}
