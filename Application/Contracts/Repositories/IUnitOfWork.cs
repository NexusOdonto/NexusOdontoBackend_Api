using Domain.Common;

namespace Application.Contracts.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUsuarioRepository Usuarios { get; }

        IRepository<T> Repository<T>() where T : BaseEntity;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();
    }
}
