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

    public async Task<EmployeeEntity?> GetByEmployeeCodeAsync(
        string employeeCode,
        CancellationToken cancellationToken = default)
    {
        var normalizedCode = employeeCode.Trim().ToUpperInvariant();
        return await Set.FirstOrDefaultAsync(e => e.EmployeeCode == normalizedCode, cancellationToken);
    }

    public async Task<int> GetMaxSequenceForPrefixAsync(
        string prefix,
        CancellationToken cancellationToken = default)
    {
        var normalizedPrefix = prefix.Trim().ToUpperInvariant();
        var codes = await Set
            .Where(e => e.EmployeeCode.StartsWith(normalizedPrefix))
            .Select(e => e.EmployeeCode)
            .ToListAsync(cancellationToken);

        var maxSequence = 0;
        foreach (var code in codes)
        {
            if (code.Length <= normalizedPrefix.Length)
            {
                continue;
            }

            if (int.TryParse(code[normalizedPrefix.Length..], out var sequence) && sequence > maxSequence)
            {
                maxSequence = sequence;
            }
        }

        return maxSequence;
    }
}
