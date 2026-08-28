using Application.Contracts.Repositories.Catalogs;
using Domain.Common;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Catalogs;

public class CatalogRepository<TEntity> : Common.Repository<TEntity>, ICatalogRepository<TEntity>
    where TEntity : CatalogEntity
{
    public CatalogRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<TEntity?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        return await Set.FirstOrDefaultAsync(e => e.Code == code, cancellationToken);
    }
}
