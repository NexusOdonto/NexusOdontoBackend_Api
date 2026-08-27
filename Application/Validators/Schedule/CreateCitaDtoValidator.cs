using Application.DTOs.Citas;
using FluentValidation;

namespace Application.Validators.Schedule
{
    public class CreateCitaDtoValidator : AbstractValidator<CreateCitaDto>
    {
        public CreateCitaDtoValidator()
        {
            RuleFor(x => x.PacienteId)
                .NotEmpty().WithMessage("Debe indicar un paciente válido.");

            RuleFor(x => x.ProfesionalId)
                .NotEmpty().WithMessage("Debe indicar un profesional válido.");

            RuleFor(x => x.ServicioId)
                .NotEmpty().WithMessage("Debe indicar un servicio válido.");

            RuleFor(x => x.FechaHoraInicio)
                .NotEmpty().WithMessage("La fecha y hora de inicio son obligatorias.");

            RuleFor(x => x.FechaHoraFin)
                .NotEmpty().WithMessage("La fecha y hora de fin son obligatorias.")
                .GreaterThan(x => x.FechaHoraInicio).WithMessage("La hora de fin debe ser posterior a la de inicio.");

            RuleFor(x => x.MotivoConsulta)
                .MaximumLength(1000).WithMessage("El motivo de consulta no puede exceder los 1000 caracteres.");

            RuleFor(x => x.Observaciones)
                .MaximumLength(2000).WithMessage("Las observaciones no pueden exceder los 2000 caracteres.");
        }
    }
}
