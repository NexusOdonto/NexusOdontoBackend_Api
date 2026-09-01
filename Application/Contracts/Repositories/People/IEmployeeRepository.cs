using Application.Contracts.Repositories.Common;
using Domain.Entities.People;

namespace Application.Contracts.Repositories.People;

public interface IEmployeeRepository : IRepository<EmployeeEntity>
{
    Task<EmployeeEntity?> GetByPersonIdAsync(
        Guid personId,
        CancellationToken cancellationToken = default);

    Task<EmployeeEntity?> GetByEmployeeCodeAsync(
        string employeeCode,
        CancellationToken cancellationToken = default);

    Task<int> GetMaxSequenceForPrefixAsync(
        string prefix,
        CancellationToken cancellationToken = default);
}
