using Application.Contracts.Repositories.People;
using Domain.Entities.People;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.People;

public sealed class EmployeeRepository : Common.Repository<EmployeeEntity>, IEmployeeRepository
{
    public EmployeeRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<EmployeeEntity?> GetByPersonIdAsync(
        Guid personId,
        CancellationToken cancellationToken = default)
    {
        return await Set.FirstOrDefaultAsync(e => e.PersonId == personId, cancellationToken);
    }
}
