using Application.Contracts.Repositories;
using Application.Contracts.Services.Clinical;
using Application.DTOs.Clinical.PatientAntecedent;
using Domain.Entities.Clinical;

namespace Application.Services.Clinical;

public sealed class PatientAntecedentService(
    IRepository<PatientAntecedent> repository,
    IUnitOfWork unitOfWork) : IPatientAntecedentService
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

    public async Task<object> CreateAsync(CreatePatientAntecedentRequestDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new PatientAntecedent(
                request.PatientId,
                request.AntecedentTypeId,
                request.Description,
                Guid.Empty);
            await repository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<object?> UpdateAsync(Guid id, UpdatePatientAntecedentRequestDto request)
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

    private static object Map(PatientAntecedent e) => new
    {
        e.Id, e.PatientId, e.AntecedentTypeId, e.Description, e.IsActive, e.RegisteredByUserId, e.RegisteredAt
    };
}
