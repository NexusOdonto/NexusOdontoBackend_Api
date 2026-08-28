using Application.Contracts.Repositories.Common;
using Domain.Common;

namespace Application.Contracts.Repositories.Catalogs;

public interface ICatalogRepository<T> : IRepository<T>
    where T : CatalogEntity
{
    Task<T?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default);
}
