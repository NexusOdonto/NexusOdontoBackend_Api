using Application.Contracts.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;
using System.Runtime.CompilerServices;

namespace Infrastructure.UnitOfWork
{
    public sealed class UnitOfWork : IUnitOfWork, IAsyncDisposable
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(AppDbContext context, IDbContextTransaction transaction)
        {
            _context = context;
            _transaction = transaction;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction is not null) 
            {
                throw new InvalidOperationException(
                    "Ya Existe una opreacion activa");
            }
            
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if(_transaction is null)
            {
                throw new InvalidOperationException(
                    "No hay ninguna transacción activa por confirmar");
            }

            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();

            _transaction = null;
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction is null)
            {
                return;
            }

            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();

            _transaction = null;
        }

        public async ValueTask DisposeAsync()
        {
            if ( _transaction is not null)
            {
                await _transaction.DisposeAsync();
            }
        }
    }
}
