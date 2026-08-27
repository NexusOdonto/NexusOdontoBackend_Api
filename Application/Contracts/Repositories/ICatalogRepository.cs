using Domain.Common;

namespace Application.Contracts.Repositories;

public interface ICatalogRepository<T> : IRepository<T>
    where T : CatalogEntity
{
    Task<T?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default);
}
