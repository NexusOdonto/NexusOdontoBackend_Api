using Application.Common;
using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.People;
using Application.Contracts.Services.People;
using Application.DTOs.People;
using Application.Exceptions;
using Domain.Entities.People;

namespace Application.Services.People;

public sealed class PersonService(
    IPersonRepository personRepository,
    IUnitOfWork unitOfWork) : IPersonService
{
    public async Task<IReadOnlyList<PersonResponseDto>> GetAllAsync()
    {
        var items = await personRepository.GetAllAsync();
        return items.Select(Map).ToList();
    }

    public async Task<PersonResponseDto?> GetByIdAsync(Guid id)
    {
        var entity = await personRepository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<PersonResponseDto> CreateAsync(CreatePersonDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var documentNumber = request.DocumentNumber.Trim();
        var existingPerson = await personRepository.GetByDocumentNumberAsync(documentNumber);
        if (existingPerson is not null)
        {
            throw new ConflictException("A person with this document number already exists.");
        }

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var person = new PersonEntity(
                request.DocumentTypeId,
                documentNumber,
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

    public async Task<PersonResponseDto?> UpdateAsync(Guid id, UpdatePersonDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await personRepository.GetByIdAsync(id);
        if (entity is null) return null;

        var documentNumber = request.DocumentNumber.Trim();
        var existingPerson = await personRepository.GetByDocumentNumberAsync(documentNumber);
        if (existingPerson is not null && existingPerson.Id != id)
        {
            throw new ConflictException("A person with this document number already exists.");
        }

        if (SeedAdminGuard.IsProtectedPerson(entity.Id) && !request.IsActive)
        {
            throw new BusinessRuleException(SeedAdminGuard.CannotDeactivateOrDeleteMessage);
        }

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.DocumentTypeId = request.DocumentTypeId;
            entity.DocumentNumber = documentNumber;
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

        if (SeedAdminGuard.IsProtectedPerson(entity.Id))
        {
            throw new BusinessRuleException(SeedAdminGuard.CannotDeactivateOrDeleteMessage);
        }

        await ServiceWrite.InTransactionAsync(unitOfWork, () =>
        {
            entity.Deactivate();
            personRepository.Update(entity);
            return Task.CompletedTask;
        });
        return true;
    }

    private static PersonResponseDto Map(PersonEntity e) => new()
    {
        Id = e.Id,
        DocumentTypeId = e.DocumentTypeId,
        DocumentNumber = e.DocumentNumber,
        FirstName = e.FirstName,
        LastName = e.LastName,
        DateOfBirth = e.DateOfBirth,
        SexId = e.SexId,
        Phone = e.Phone,
        Email = e.Email,
        Address = e.Address,
        IsActive = e.IsActive,
        CreatedAt = e.CreatedAt
    };
}
