using Application.DTOs.Availabilities;
using FluentValidation;

namespace Application.Validators.Schedule
{
    public class CreateAvailabilityDtoValidator : AbstractValidator<CreateAvailabilityDto>
    {
        public CreateAvailabilityDtoValidator()
        {
            RuleFor(x => x.ProfessionalId)
                .NotEmpty().WithMessage("Debe indicar un profesional válido.");

            RuleFor(x => x.EndTime)
                .GreaterThan(x => x.StartTime)
                .WithMessage("La hora de fin debe ser posterior a la de inicio.");
        }
    }
}
