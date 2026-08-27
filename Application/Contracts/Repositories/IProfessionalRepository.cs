using Domain.Entities.People;

namespace Application.Contracts.Repositories;

public interface IProfessionalRepository : IRepository<Professional>
{
    Task<Professional?> GetByEmployeeIdAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default);

    Task<Professional?> GetByLicenseAsync(
        string professionalLicense,
        CancellationToken cancellationToken = default);

    Task<Professional?> GetByIdWithSpecialtiesAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
