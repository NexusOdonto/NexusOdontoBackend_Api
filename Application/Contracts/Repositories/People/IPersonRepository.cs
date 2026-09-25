using Application.Contracts.Repositories.Common;
using Domain.Entities.People;

namespace Application.Contracts.Repositories.People;

public interface IPersonRepository : IRepository<PersonEntity>
{
    Task<PersonEntity?> GetByDocumentAsync(
        Guid documentTypeId,
        string documentNumber,
        CancellationToken cancellationToken = default);

    Task<PersonEntity?> GetByDocumentNumberAsync(
        string documentNumber,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Document lookup that includes soft-deleted persons (IsActive=false).
    /// Used to block recreation of deactivated identities (e.g. chatbot orphans).
    /// </summary>
    Task<PersonEntity?> GetByDocumentNumberIncludingInactiveAsync(
        string documentNumber,
        CancellationToken cancellationToken = default);

    Task<PersonEntity?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);
}
