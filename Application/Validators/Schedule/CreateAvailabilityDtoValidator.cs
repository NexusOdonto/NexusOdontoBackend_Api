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
                .Must((dto, endTime) => IsEndTimeAfterStartTime(dto.StartTime, endTime))
                .WithMessage("La hora de fin debe ser posterior a la de inicio.");
        }

        private static bool IsEndTimeAfterStartTime(string startTime, string endTime)
        {
            if (!TimeSpan.TryParse(startTime, out var start) || !TimeSpan.TryParse(endTime, out var end))
                return true;

            return end > start;
        }
    }
}
