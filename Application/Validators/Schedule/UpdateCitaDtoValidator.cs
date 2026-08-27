using Application.DTOs.Citas;
using FluentValidation;

namespace Application.Validators.Schedule
{
    public class UpdateCitaDtoValidator : AbstractValidator<UpdateCitaDto>
    {
        public UpdateCitaDtoValidator()
        {
            RuleFor(x => x.FechaHoraInicio)
                .NotEmpty().WithMessage("La fecha y hora de inicio son obligatorias.");

            RuleFor(x => x.FechaHoraFin)
                .NotEmpty().WithMessage("La fecha y hora de fin son obligatorias.")
                .GreaterThan(x => x.FechaHoraInicio).WithMessage("La hora de fin debe ser posterior a la de inicio.");
        }
    }
}
