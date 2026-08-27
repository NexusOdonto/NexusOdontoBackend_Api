using Domain.Entities.People;

namespace Application.Contracts.Repositories;

public interface IPersonRepository : IRepository<Person>
{
    Task<Person?> GetByDocumentAsync(
        Guid documentTypeId,
        string documentNumber,
        CancellationToken cancellationToken = default);

    Task<Person?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);
}
