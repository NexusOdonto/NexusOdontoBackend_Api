using Application.Contracts.Repositories.Common;
using Domain.Entities.People;

namespace Application.Contracts.Repositories.People;

public interface IUserRepository : IRepository<UserEntity>
{
    Task<UserEntity?> GetByEmailWithRolesAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task<string?> GetEmailByDocumentNumberAsync(
        string documentNumber,
        CancellationToken cancellationToken = default);

    Task<UserEntity?> GetByPersonIdAsync(
        Guid personId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<UserEntity>> GetAllWithRolesAsync(
        CancellationToken cancellationToken = default);

    Task<UserEntity?> GetByIdWithRolesAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
