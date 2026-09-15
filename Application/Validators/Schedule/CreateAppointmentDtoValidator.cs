using Application.DTOs.Appointments;
using FluentValidation;

namespace Application.Validators.Schedule
{
    public class CreateAppointmentDtoValidator : AbstractValidator<CreateAppointmentDto>
    {
        public CreateAppointmentDtoValidator()
        {
            RuleFor(x => x.PatientId)
                .NotEmpty().WithMessage("Debe indicar un paciente válido.");

            When(x => !x.AutoAssignProfessional, () =>
            {
                RuleFor(x => x.ProfessionalId)
                    .NotEmpty().WithMessage("Debe indicar un profesional válido.");
            });

            RuleFor(x => x.StartsAt)
                .NotEmpty().WithMessage("La fecha y hora de inicio son obligatorias.");

            RuleFor(x => x.EndsAt)
                .NotEmpty().WithMessage("La fecha y hora de fin son obligatorias.")
                .GreaterThan(x => x.StartsAt).WithMessage("La hora de fin debe ser posterior a la de inicio.");

            RuleFor(x => x.ReasonForVisit)
                .MaximumLength(1000).WithMessage("El motivo de consulta no puede exceder los 1000 caracteres.");

            RuleFor(x => x.Notes)
                .MaximumLength(2000).WithMessage("Las observaciones no pueden exceder los 2000 caracteres.");
        }
    }
}
