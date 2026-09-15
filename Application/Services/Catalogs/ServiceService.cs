using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.Catalogs;
using Application.Contracts.Services.Catalogs;
using Application.DTOs;
using Application.DTOs.Service;
using Application.Exceptions;
using Domain.Entities.Catalogs;

namespace Application.Services.Catalogs;

public sealed class ServiceService(
    IServiceRepository repository,
    IUnitOfWork unitOfWork) : IServiceService
{
    public async Task<IReadOnlyList<ServiceResponseDto>> GetAllAsync()
    {
        var items = await repository.GetAllAsync();
        return items.Select(Map).ToList();
    }

    public async Task<ServiceResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<ServiceResponseDto> CreateAsync(CreateServiceRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var code = await ResolveServiceCodeAsync(request);
            var entity = new ServiceEntity(
                code,
                request.Name,
                request.Description,
                request.DurationMinutes,
                request.Price,
                request.Category,
                request.SortOrder,
                request.IsActive);
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

    public async Task<ServiceResponseDto?> UpdateAsync(Guid id, CreateServiceRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return null;

        await unitOfWork.BeginTransactionAsync();
        try
        {
            if (!string.IsNullOrWhiteSpace(request.Code))
            {
                var normalizedCode = ServiceCodeGenerator.NormalizeCode(request.Code);
                if (!string.Equals(normalizedCode, entity.Code, StringComparison.Ordinal))
                {
                    var duplicate = await repository.GetByCodeAsync(normalizedCode);
                    if (duplicate is not null)
                    {
                        throw new ConflictException("Service code is already in use.");
                    }

                    entity.Code = normalizedCode;
                }
            }

            entity.Update(
                request.Name,
                request.Description,
                request.DurationMinutes,
                request.Price,
                request.Category,
                request.SortOrder,
                request.IsActive);
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

    private async Task<string> ResolveServiceCodeAsync(CreateServiceRequestDto request)
    {
        if (!string.IsNullOrWhiteSpace(request.Code))
        {
            var normalizedCode = ServiceCodeGenerator.NormalizeCode(request.Code);
            var existing = await repository.GetByCodeAsync(normalizedCode);
            if (existing is not null)
            {
                throw new ConflictException("Service code is already in use.");
            }

            return normalizedCode;
        }

        var baseAcronym = ServiceCodeGenerator.FromName(request.Name);
        var existingBase = await repository.GetByCodeAsync(baseAcronym);
        if (existingBase is null)
        {
            return baseAcronym;
        }

        var prefix = ServiceCodeGenerator.GetCollisionPrefix(baseAcronym);
        var nextSequence = await repository.GetMaxSequenceForPrefixAsync(prefix) + 1;
        return ServiceCodeGenerator.FormatWithSuffix(prefix, nextSequence);
    }

    private static ServiceResponseDto Map(ServiceEntity e) => new()
    {
        Id = e.Id,
        Code = e.Code,
        Name = e.Name,
        Description = e.Description,
        DurationMinutes = e.DurationMinutes,
        Price = e.Price,
        Category = e.Category,
        IsActive = e.IsActive,
        SortOrder = e.SortOrder,
        CreatedAt = e.CreatedAt
    };
}
