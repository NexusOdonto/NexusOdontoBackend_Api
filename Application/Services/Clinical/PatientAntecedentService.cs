using Application.Contracts.Repositories.Common;
using Application.Contracts.Services.Clinical;
using Application.DTOs.Clinical.PatientAntecedent;
using Domain.Entities.Clinical;

namespace Application.Services.Clinical;

public sealed class PatientAntecedentService(
    IRepository<PatientAntecedentEntity> repository,
    IUnitOfWork unitOfWork) : IPatientAntecedentService
{
    public async Task<IReadOnlyList<PatientAntecedentResponseDto>> GetAllAsync()
    {
        var items = await repository.GetAllAsync();
        return items.Select(Map).ToList();
    }

    public async Task<PatientAntecedentResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<PatientAntecedentResponseDto> CreateAsync(CreatePatientAntecedentRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var registeredBy = request.RegisteredByUserId.HasValue && request.RegisteredByUserId.Value != Guid.Empty
                ? request.RegisteredByUserId.Value
                : new Guid("1b000000-0000-0000-0000-000000000002");

            var entity = new PatientAntecedentEntity(
                request.PatientId,
                request.AntecedentTypeId,
                request.Description,
                registeredBy);
            await repository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<PatientAntecedentResponseDto?> UpdateAsync(Guid id, UpdatePatientAntecedentRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.AntecedentTypeId = request.AntecedentTypeId;
            entity.Update(request.Description, request.IsActive);
            repository.Update(entity);
            return Map(entity);
        });
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return false;
        await ServiceWrite.InTransactionAsync(unitOfWork, () =>
        {
            entity.Update(entity.Description, false);
            repository.Update(entity);
            return Task.CompletedTask;
        });
        return true;
    }

    private static PatientAntecedentResponseDto Map(PatientAntecedentEntity e) => new()
    {
        Id = e.Id,
        PatientId = e.PatientId,
        AntecedentTypeId = e.AntecedentTypeId,
        Description = e.Description,
        IsActive = e.IsActive,
        RegisteredByUserId = e.RegisteredByUserId,
        RegisteredAt = e.RegisteredAt
    };
}
