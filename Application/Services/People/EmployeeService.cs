using Application.Contracts.Repositories.Common;
using Application.Contracts.Repositories.People;
using Application.Contracts.Services.People;
using Application.DTOs.Empleados;
using Domain.Entities.People;

namespace Application.Services.People;

public sealed class EmployeeService(
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork) : IEmployeeService
{
    public async Task<IReadOnlyList<object>> GetAllAsync()
    {
        var items = await employeeRepository.GetAllAsync();
        return items.Select(Map).Cast<object>().ToList();
    }

    public async Task<object?> GetByIdAsync(Guid id)
    {
        var entity = await employeeRepository.GetByIdAsync(id);
        return entity is null ? null : Map(entity);
    }

    public async Task<object> CreateAsync(CreateEmpleadoDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            var entity = new EmployeeEntity(request.PersonaId, request.CargoId, request.FechaVinculacion);
            await employeeRepository.AddAsync(entity);
            return Map(entity);
        });
    }

    public async Task<object?> UpdateAsync(Guid id, UpdateEmpleadoDto request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var entity = await employeeRepository.GetByIdAsync(id);
        if (entity is null) return null;

        return await ServiceWrite.InTransactionAsync(unitOfWork, async () =>
        {
            entity.Update(request.CargoId, request.FechaVinculacion);
            entity.IsActive = request.Activo;
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

    private static object Map(EmployeeEntity e) => new
    {
        e.Id,
        e.PersonId,
        e.JobTitleId,
        e.HireDate,
        e.IsActive,
        e.CreatedAt
    };
}
