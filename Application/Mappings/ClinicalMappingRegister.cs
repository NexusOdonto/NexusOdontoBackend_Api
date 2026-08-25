using Application.DTOs;
using Domain.Entities;
using Mapster;

namespace Application.Mappings;

public sealed class ClinicalMappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Un paciente nuevo genera dos entidades: la persona y el paciente.
        config.NewConfig<CrearPacienteRequestDto, Persona>();

        // PersonaId lo asigna el servicio tras persistir la persona.
        config.NewConfig<CrearPacienteRequestDto, Paciente>()
            .Ignore(dest => dest.PersonaId);

        config.NewConfig<CrearServicioRequestDto, Servicio>();
    }
}
