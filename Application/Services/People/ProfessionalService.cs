using Application.Contracts.Repositories;
using Application.Contracts.Services.People;
using Application.DTOs.Profesionales;
using Domain.Entities.People;

namespace Application.Services.People;

public sealed class ProfessionalService(
    IProfessionalRepository professionalRepository,
    IUnitOfWork unitOfWork) : IProfessionalService
{
    public async Task<IReadOnlyList<object>> GetAllAsync()
    {
        var items = await professionalRepository.GetAllAsync();
        return items.Select(Map).Cast<object>().ToList();
    }

    public async Task<object?> GetByIdAsync(Guid id)
    {
        var entity = await professionalRepository.GetByIdWithSpecialtiesAsync(id)
            ?? await professionalRepository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<object> CreateAsync(CreateProfesionalDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new Professional(request.EmpleadoId, request.RegistroProfesional);
            await professionalRepository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<object?> UpdateAsync(Guid id, UpdateProfesionalDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await professionalRepository.GetByIdAsync(id);
        if (entity is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.Update(request.RegistroProfesional);
            entity.IsActive = request.Activo;
            professionalRepository.Update(entity);
            return Map(entity);
        });
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await professionalRepository.GetByIdAsync(id);
        if (entity is null) return false;

        await ServiceWrite.InTransactionAsync(unitOfWork, () =>
        {
            entity.Deactivate();
            professionalRepository.Update(entity);
            return Task.CompletedTask;
        });
        return true;
    }

    private static object Map(Professional e) => new
    {
        e.Id,
        e.EmployeeId,
        e.ProfessionalLicense,
        SpecialtyIds = e.Specialties.Select(s => s.Id).ToList(),
        e.IsActive,
        e.CreatedAt
    };
}
