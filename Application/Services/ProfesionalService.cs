using Application.Contracts.Repositories;
using Domain.Entities;

namespace Application.Services;

public sealed class ProfesionalService(
    IRepository<Empleado> empleadoRepository,
    IRepository<Profesional> profesionalRepository,
    IRepository<ProfesionalEspecialidad> profesionalEspecialidadRepository,
    IUnitOfWork unitOfWork)
{
    public async Task<Guid> CrearAsync(
        Guid personaId,
        Guid cargoId,
        DateTime fechaVinculacion,
        string registroProfesional,
        IEnumerable<Guid> especialidadIds,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(especialidadIds);

        await unitOfWork.BeginTransactionAsync();

        try
        {
            var empleado = new Empleado
            {
                PersonaId = personaId,
                CargoId = cargoId,
                FechaVinculacion = fechaVinculacion
            };
            await empleadoRepository.AddAsync(empleado);

            var profesional = new Profesional
            {
                EmpleadoId = empleado.Id,
                RegistroProfesional = registroProfesional
            };
            await profesionalRepository.AddAsync(profesional);

            foreach (var especialidadId in especialidadIds)
            {
                await profesionalEspecialidadRepository.AddAsync(
                    new ProfesionalEspecialidad
                    {
                        ProfesionalId = profesional.Id,
                        EspecialidadId = especialidadId
                    });
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync();

            return profesional.Id;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
