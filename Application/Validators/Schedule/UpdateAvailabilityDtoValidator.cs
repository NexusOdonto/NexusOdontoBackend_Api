using Application.DTOs.Availabilities;
using FluentValidation;

namespace Application.Validators.Schedule
{
    public class UpdateAvailabilityDtoValidator : AbstractValidator<UpdateAvailabilityDto>
    {
        public UpdateAvailabilityDtoValidator()
        {
            RuleFor(x => x.EndTime)
                .GreaterThan(x => x.StartTime)
                .WithMessage("La hora de fin debe ser posterior a la de inicio.");
        }
    }
}
