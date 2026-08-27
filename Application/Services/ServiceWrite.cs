using Application.Contracts.Repositories;
using Domain.Common;

namespace Application.Services;

internal static class ServiceWrite
{
    public static async Task<TResult> InTransactionAsync<TResult>(
        IUnitOfWork unitOfWork,
        Func<Task<TResult>> action)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var result = await action();
            await unitOfWork.SaveChangesAsync();
            await unitOfWork.CommitTransactionAsync();
            return result;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public static async Task InTransactionAsync(
        IUnitOfWork unitOfWork,
        Func<Task> action)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            await action();
            await unitOfWork.SaveChangesAsync();
            await unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public static async Task<Guid> ResolveCatalogIdAsync<T>(
        ICatalogRepository<T> catalog,
        params string[] preferredCodes)
        where T : CatalogEntity
    {
        foreach (var code in preferredCodes)
        {
            var byCode = await catalog.GetByCodeAsync(code);
            if (byCode is not null)
            {
                return byCode.Id;
            }
        }

        var all = await catalog.GetAllAsync();
        var first = all.FirstOrDefault(x => x.IsActive) ?? all.FirstOrDefault();
        if (first is null)
        {
            throw new InvalidOperationException($"No catalog entries found for {typeof(T).Name}.");
        }

        return first.Id;
    }
}
