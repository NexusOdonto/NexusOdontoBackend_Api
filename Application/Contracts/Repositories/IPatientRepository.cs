using Domain.Entities.People;

namespace Application.Contracts.Repositories;

public interface IPatientRepository : IRepository<Patient>
{
    Task<Patient?> GetByPersonIdAsync(
        Guid personId,
        CancellationToken cancellationToken = default);

    Task<Patient?> GetByIdWithPersonAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
