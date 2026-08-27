using Application.Contracts.Repositories;
using Application.Contracts.Services.People;
using Application.DTOs.Personas;
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

    public async Task<object> CreateAsync(CreatePersonaDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new Person(
                request.TipoDocumentoId,
                request.NumeroDocumento,
                request.Nombres,
                request.Apellidos,
                request.FechaNacimiento,
                request.SexoId,
                request.Telefono,
                request.Email,
                request.Direccion);
            await personRepository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<object?> UpdateAsync(Guid id, UpdatePersonaDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await personRepository.GetByIdAsync(id);
        if (entity is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.DocumentTypeId = request.TipoDocumentoId;
            entity.DocumentNumber = request.NumeroDocumento;
            entity.Update(
                request.Nombres,
                request.Apellidos,
                request.FechaNacimiento,
                request.SexoId,
                request.Telefono,
                request.Email,
                request.Direccion);
            entity.IsActive = request.Activo;
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

    private static object Map(Person e) => new
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

