using Application.Contracts.Repositories;
using Application.DTOs;
using Domain.Entities;
using MapsterMapper;

namespace Application.Services;

public sealed class PacienteService(
    IRepository<Persona> personaRepository,
    IRepository<Paciente> pacienteRepository,
    IRepository<HistoriaClinica> historiaClinicaRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper)
{
    public async Task<Guid> CrearAsync(
        CrearPacienteRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        await unitOfWork.BeginTransactionAsync();

        try
        {
            var persona = mapper.Map<Persona>(request);
            await personaRepository.AddAsync(persona);

            var paciente = mapper.Map<Paciente>(request);
            paciente.PersonaId = persona.Id;
            await pacienteRepository.AddAsync(paciente);

            // El DoD exige que todo paciente nuevo abra su historia clinica.
            var historiaClinica = new HistoriaClinica
            {
                PacienteId = paciente.Id,
                FechaApertura = DateTime.UtcNow,
                Activa = true
            };
            await historiaClinicaRepository.AddAsync(historiaClinica);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync();

            return paciente.Id;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
