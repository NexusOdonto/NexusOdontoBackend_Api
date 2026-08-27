using Application.DTOs.Usuarios;
using FluentValidation;

namespace Application.Validators.People
{
    public class CreateUsuarioDtoValidator : AbstractValidator<CreateUsuarioDto>
    {
        public CreateUsuarioDtoValidator()
        {
            RuleFor(x => x.PersonaId)
                .NotEmpty().WithMessage("Debe indicar una persona válida.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");
        }
    }
}
