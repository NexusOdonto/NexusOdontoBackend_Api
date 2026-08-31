using Domain.Entities.Security;

namespace Application.Contracts.Repositories.Security;

public interface IUserExternalLoginRepository
{
    Task<UserExternalLoginEntity?> GetByProviderKeyAsync(
        string provider,
        string providerKey,
        CancellationToken cancellationToken = default);

    Task<UserExternalLoginEntity?> GetByUserAndProviderAsync(
        Guid userId,
        string provider,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        UserExternalLoginEntity entity,
        CancellationToken cancellationToken = default);
}
