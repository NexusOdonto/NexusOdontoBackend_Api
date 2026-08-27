using Application.DTOs.Disponibilidades;
using FluentValidation;

namespace Application.Validators.Schedule
{
    public class CreateDisponibilidadDtoValidator : AbstractValidator<CreateDisponibilidadDto>
    {
        public CreateDisponibilidadDtoValidator()
        {
            RuleFor(x => x.ProfesionalId)
                .NotEmpty().WithMessage("Debe indicar un profesional válido.");

            RuleFor(x => x.HoraFin)
                .GreaterThan(x => x.HoraInicio).WithMessage("La hora de fin debe ser posterior a la de inicio.");
        }
    }
}
