using Domain.Entities.Odontogram;

namespace Application.Contracts.Repositories.Odontogram;

public interface IToothRepository
{
    Task<ToothEntity?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ToothEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(ToothEntity entity, CancellationToken cancellationToken = default);

    void Update(ToothEntity entity);

    void Remove(ToothEntity entity);
}
