using Domain.Entities.Catalogs;

namespace Application.Contracts.Repositories;

public interface IServiceRepository : IRepository<Service>
{
    Task<Service?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default);
}
