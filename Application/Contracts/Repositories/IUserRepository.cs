using Domain.Entities.People;

namespace Application.Contracts.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailWithRolesAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<User>> GetAllWithRolesAsync(
        CancellationToken cancellationToken = default);

    Task<User?> GetByIdWithRolesAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
