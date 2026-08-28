using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.Catalogs;
using Domain.Common;

namespace Application.Services.Catalogs;

public sealed class CatalogServiceCore<TCatalog>(
    ICatalogRepository<TCatalog> repository,
    IUnitOfWork unitOfWork)
    where TCatalog : CatalogEntity
{
    public async Task<IReadOnlyList<TDto>> GetAllAsync<TDto>(Func<TCatalog, TDto> map)
    {
        var items = await repository.GetAllAsync();
        return items.Select(map).ToList();
    }

    public async Task<TDto?> GetByIdAsync<TDto>(Guid id, Func<TCatalog, TDto> map)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity is null ? default : map(entity);
    }

    public async Task<TDto> CreateAsync<TDto>(TCatalog entity, Func<TCatalog, TDto> map)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            await repository.AddAsync(entity);
            await unitOfWork.SaveChangesAsync();
            await unitOfWork.CommitTransactionAsync();
            return map(entity);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<TDto?> UpdateAsync<TDto>(
        Guid id,
        Action<TCatalog> apply,
        Func<TCatalog, TDto> map)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null)
        {
            return default;
        }

        await unitOfWork.BeginTransactionAsync();
        try
        {
            apply(entity);
            repository.Update(entity);
            await unitOfWork.SaveChangesAsync();
            await unitOfWork.CommitTransactionAsync();
            return map(entity);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null)
        {
            return false;
        }

        await unitOfWork.BeginTransactionAsync();
        try
        {
            entity.Deactivate();
            repository.Update(entity);
            await unitOfWork.SaveChangesAsync();
            await unitOfWork.CommitTransactionAsync();
            return true;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}

