using Domain.Entities;

namespace Application.Contracts.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> GetByCorreoWithRoleAsync(
            string correo,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Usuario>> GetAllWithRoleAsync(
            CancellationToken cancellationToken = default);

        Task<Usuario?> GetByIdWithRoleAsync(
            long id,
            CancellationToken cancellationToken = default);
    }
}
