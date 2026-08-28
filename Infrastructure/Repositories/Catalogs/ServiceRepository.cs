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
        return await Set.FirstOrDefaultAsync(s => s.Code == code, cancellationToken);
    }
}
