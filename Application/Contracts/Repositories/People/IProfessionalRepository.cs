using Application.Contracts.Repositories.Common;
using Domain.Entities.People;

namespace Application.Contracts.Repositories.People;

public interface IProfessionalRepository : IRepository<ProfessionalEntity>
{
    Task<ProfessionalEntity?> GetByEmployeeIdAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default);

    Task<ProfessionalEntity?> GetByLicenseAsync(
        string professionalLicense,
        CancellationToken cancellationToken = default);

    Task<ProfessionalEntity?> GetByIdWithSpecialtiesAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
