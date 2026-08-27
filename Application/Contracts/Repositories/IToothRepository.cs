using Domain.Entities.Odontogram;

namespace Application.Contracts.Repositories;

public interface IToothRepository
{
    Task<Tooth?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Tooth>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Tooth entity, CancellationToken cancellationToken = default);

    void Update(Tooth entity);

    void Remove(Tooth entity);
}
