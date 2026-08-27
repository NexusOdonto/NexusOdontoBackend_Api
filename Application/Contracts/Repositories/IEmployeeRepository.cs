using Domain.Entities.People;

namespace Application.Contracts.Repositories;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<Employee?> GetByPersonIdAsync(
        Guid personId,
        CancellationToken cancellationToken = default);
}
