using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.People;
using Application.Contracts.Services.People;
using Application.DTOs.Employees;
using Application.Exceptions;
using Application.Services.People;
using Domain.Entities.Catalogs;
using Domain.Entities.People;

namespace Application.Services.People;

public sealed class EmployeeService(
    IEmployeeRepository employeeRepository,
    IRepository<JobTitleEntity> jobTitleRepository,
    IUnitOfWork unitOfWork) : IEmployeeService
{
    public async Task<IReadOnlyList<EmployeeResponseDto>> GetAllAsync()
    {
        var items = await employeeRepository.GetAllAsync();
        return items.Select(Map).ToList();
    }

    public async Task<EmployeeResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await employeeRepository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<EmployeeResponseDto> CreateAsync(CreateEmployeeDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var employeeCode = await ResolveEmployeeCodeAsync(request);
            var entity = new EmployeeEntity(
                request.PersonId,
                request.JobTitleId,
                request.HireDate,
                employeeCode);
            await employeeRepository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<EmployeeResponseDto?> UpdateAsync(Guid id, UpdateEmployeeDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await employeeRepository.GetByIdAsync(id);
        if (entity is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.Update(request.JobTitleId, request.HireDate, entity.EmployeeCode);
            entity.IsActive = request.IsActive;
            employeeRepository.Update(entity);
            return Map(entity);
        });
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await employeeRepository.GetByIdAsync(id);
        if (entity is null) return false;

        await ServiceWrite.InTransactionAsync(unitOfWork, () =>
        {
            entity.Deactivate();
            employeeRepository.Update(entity);
            return Task.CompletedTask;
        });
        return true;
    }

    private async Task<string> ResolveEmployeeCodeAsync(CreateEmployeeDto request)
    {
        if (!string.IsNullOrWhiteSpace(request.EmployeeCode))
        {
            var normalizedCode = EmployeeCodeGenerator.NormalizeEmployeeCode(request.EmployeeCode);
            var existing = await employeeRepository.GetByEmployeeCodeAsync(normalizedCode);
            if (existing is not null)
            {
                throw new ConflictException("Employee code is already in use.");
            }

            return normalizedCode;
        }

        var jobTitle = await jobTitleRepository.GetByIdAsync(request.JobTitleId)
            ?? throw new KeyNotFoundException("Job title not found.");

        var prefix = EmployeeCodeGenerator.GetPrefixForJobTitle(jobTitle.Code)
            ?? throw new InvalidOperationException(
                $"Job title '{jobTitle.Code}' does not support automatic employee code generation.");

        var nextSequence = await employeeRepository.GetMaxSequenceForPrefixAsync(prefix) + 1;
        return EmployeeCodeGenerator.FormatCode(prefix, nextSequence);
    }

    private static EmployeeResponseDto Map(EmployeeEntity e) => new()
    {
        Id = e.Id,
        PersonId = e.PersonId,
        JobTitleId = e.JobTitleId,
        EmployeeCode = e.EmployeeCode,
        HireDate = e.HireDate,
        IsActive = e.IsActive,
        CreatedAt = e.CreatedAt
    };
}
