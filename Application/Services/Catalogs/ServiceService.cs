using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.Catalogs;
using Application.Contracts.Services.Catalogs;
using Application.DTOs;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class ServiceService(
    IServiceRepository repository,
    IUnitOfWork unitOfWork) : IServiceService
{
    public async Task<IReadOnlyList<object>> GetAllAsync()
    {
        var items = await repository.GetAllAsync();
        return items.Select(Map).Cast<object>().ToList();
    }

    public async Task<object?> GetByIdAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<object> CreateAsync(CreateServiceRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var entity = new ServiceEntity(
                request.Code,
                request.Name,
                request.Description,
                request.DurationMinutes,
                request.Price,
                request.SortOrder);
            await repository.AddAsync(entity);
            await unitOfWork.SaveChangesAsync();
            await unitOfWork.CommitTransactionAsync();
            return Map(entity);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<object?> UpdateAsync(Guid id, CreateServiceRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return null;

        await unitOfWork.BeginTransactionAsync();
        try
        {
            entity.Code = request.Code;
            entity.Update(request.Name, request.Description, request.DurationMinutes, request.Price, request.SortOrder);
            repository.Update(entity);
            await unitOfWork.SaveChangesAsync();
            await unitOfWork.CommitTransactionAsync();
            return Map(entity);
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
        if (entity is null) return false;

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

    private static object Map(ServiceEntity e) => new
    {
        e.Id,
        e.Code,
        e.Name,
        e.Description,
        e.DurationMinutes,
        e.Price,
        e.IsActive,
        e.SortOrder,
        e.CreatedAt
    };
}
