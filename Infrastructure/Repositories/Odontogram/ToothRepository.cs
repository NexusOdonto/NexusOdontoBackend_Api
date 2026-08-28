using Application.Contracts.Repositories.Odontogram;
using Domain.Entities.Odontogram;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Odontogram;

public sealed class ToothRepository : IToothRepository
{
    private readonly AppDbContext _context;

    public ToothRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ToothEntity?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<ToothEntity>()
            .FirstOrDefaultAsync(t => t.Code == code, cancellationToken);
    }

    public async Task<IReadOnlyList<ToothEntity>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Set<ToothEntity>().ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ToothEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Set<ToothEntity>().AddAsync(entity, cancellationToken);
    }

    public void Update(ToothEntity entity)
    {
        _context.Set<ToothEntity>().Update(entity);
    }

    public void Remove(ToothEntity entity)
    {
        _context.Set<ToothEntity>().Remove(entity);
    }
}
