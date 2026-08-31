using Application.Contracts.Repositories.Common;
using Domain.Common;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Common;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : Entity
{
    protected readonly AppDbContext Context;
    protected DbSet<TEntity> Set => Context.Set<TEntity>();

    public Repository(AppDbContext context)
    {
        Context = context;
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await Set.FirstOrDefaultAsync(e => e.Id == id);
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync()
    {
        return await Set.ToListAsync();
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await Set.AddAsync(entity);
    }

    public virtual void Update(TEntity entity)
    {
        Set.Update(entity);
    }

    public virtual void Remove(TEntity entity)
    {
        Set.Remove(entity);
    }
}
