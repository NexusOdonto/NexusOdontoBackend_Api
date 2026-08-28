using Application.Contracts.Repositories.Common;
using Domain.Entities.People;

namespace Application.Contracts.Repositories.People;

public interface IPatientRepository : IRepository<PatientEntity>
{
    Task<PatientEntity?> GetByPersonIdAsync(
        Guid personId,
        CancellationToken cancellationToken = default);

    Task<PatientEntity?> GetByIdWithPersonAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
