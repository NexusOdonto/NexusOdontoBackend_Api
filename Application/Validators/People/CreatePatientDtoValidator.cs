using Application.DTOs.Patients;
using FluentValidation;

namespace Application.Validators.People
{
    public class CreatePatientDtoValidator : AbstractValidator<CreatePatientDto>
    {
        public CreatePatientDtoValidator()
        {
            RuleFor(x => x.PersonId)
                .NotEmpty().WithMessage("Debe indicar una persona válida.");

            RuleFor(x => x.EmergencyContact)
                .MaximumLength(150).WithMessage("El contacto de emergencia no puede exceder 150 caracteres.");

            RuleFor(x => x.EmergencyPhone)
                .MaximumLength(30).WithMessage("El teléfono de emergencia no puede exceder 30 caracteres.");
        }
    }
}
