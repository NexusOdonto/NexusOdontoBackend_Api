using Application.Contracts.Repositories.Catalogs;
using Domain.Entities.Catalogs;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Catalogs;

public sealed class ServiceRepository : Common.Repository<ServiceEntity>, IServiceRepository
{
    public ServiceRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<ServiceEntity?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim().ToUpperInvariant();
        return await Set.FirstOrDefaultAsync(s => s.Code == normalizedCode, cancellationToken);
    }

    public async Task<int> GetMaxSequenceForPrefixAsync(
        string prefix,
        CancellationToken cancellationToken = default)
    {
        var normalizedPrefix = prefix.Trim().ToUpperInvariant();
        var codes = await Set
            .Where(s => s.Code.StartsWith(normalizedPrefix))
            .Select(s => s.Code)
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
