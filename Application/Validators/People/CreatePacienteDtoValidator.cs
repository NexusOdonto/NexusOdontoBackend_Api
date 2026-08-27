using Application.DTOs.Pacientes;
using FluentValidation;

namespace Application.Validators.People
{
    public class CreatePacienteDtoValidator : AbstractValidator<CreatePacienteDto>
    {
        public CreatePacienteDtoValidator()
        {
            RuleFor(x => x.PersonaId)
                .NotEmpty().WithMessage("Debe indicar una persona válida.");

            RuleFor(x => x.ContactoEmergencia)
                .MaximumLength(150).WithMessage("El contacto de emergencia no puede exceder 150 caracteres.");

            RuleFor(x => x.TelefonoEmergencia)
                .MaximumLength(30).WithMessage("El teléfono de emergencia no puede exceder 30 caracteres.");
        }
    }
}
