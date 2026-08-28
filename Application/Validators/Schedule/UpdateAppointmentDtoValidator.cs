using Application.DTOs.Appointments;
using FluentValidation;

namespace Application.Validators.Schedule
{
    public class UpdateAppointmentDtoValidator : AbstractValidator<UpdateAppointmentDto>
    {
        public UpdateAppointmentDtoValidator()
        {
            RuleFor(x => x.StartsAt)
                .NotEmpty().WithMessage("La fecha y hora de inicio son obligatorias.");

            RuleFor(x => x.EndsAt)
                .NotEmpty().WithMessage("La fecha y hora de fin son obligatorias.")
                .GreaterThan(x => x.StartsAt).WithMessage("La hora de fin debe ser posterior a la de inicio.");
        }
    }
}
