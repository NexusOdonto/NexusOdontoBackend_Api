using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.People;
using Application.Contracts.Services.People;
using Application.DTOs.People;
using Domain.Entities.People;

namespace Application.Services.People;

public sealed class PersonService(
    IPersonRepository personRepository,
    IUnitOfWork unitOfWork) : IPersonService
{
    public async Task<IReadOnlyList<object>> GetAllAsync()
    {
        var items = await personRepository.GetAllAsync();
        return items.Select(Map).Cast<object>().ToList();
    }

    public async Task<object?> GetByIdAsync(Guid id)
    {
        var entity = await personRepository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<object> CreateAsync(CreatePersonDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var person = new PersonEntity(
                request.DocumentTypeId,
                request.DocumentNumber,
                request.FirstName,
                request.LastName,
                request.DateOfBirth,
                request.SexId,
                request.Phone,
                request.Email,
                request.Address);
            await personRepository.AddAsync(person);
            return Map(person);
        });
    }

    public async Task<object?> UpdateAsync(Guid id, UpdatePersonDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await personRepository.GetByIdAsync(id);
        if (entity is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.DocumentTypeId = request.DocumentTypeId;
            entity.DocumentNumber = request.DocumentNumber;
            entity.Update(
                request.FirstName,
                request.LastName,
                request.DateOfBirth,
                request.SexId,
                request.Phone,
                request.Email,
                request.Address);
            entity.IsActive = request.IsActive;
            personRepository.Update(entity);
            return Map(entity);
        });
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await personRepository.GetByIdAsync(id);
        if (entity is null) return false;

        await ServiceWrite.InTransactionAsync(unitOfWork, () =>
        {
            entity.Deactivate();
            personRepository.Update(entity);
            return Task.CompletedTask;
        });
        return true;
    }

    private static object Map(PersonEntity e) => new
    {
        e.Id,
        e.DocumentTypeId,
        e.DocumentNumber,
        e.FirstName,
        e.LastName,
        e.DateOfBirth,
        e.SexId,
        e.Phone,
        e.Email,
        e.Address,
        e.IsActive,
        e.CreatedAt
    };
}
