using Application.Contracts.Repositories.Common;
using Domain.Entities.Catalogs;

namespace Application.Contracts.Repositories.Catalogs;

public interface IServiceRepository : IRepository<ServiceEntity>
{
    Task<ServiceEntity?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default);
}
