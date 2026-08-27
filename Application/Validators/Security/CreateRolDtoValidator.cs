using Application.DTOs.Roles;
using FluentValidation;

namespace Application.Validators.Security
{
    public class CreateRolDtoValidator : AbstractValidator<CreateRolDto>
    {
        public CreateRolDtoValidator()
        {
            RuleFor(x => x.Codigo)
                .NotEmpty().WithMessage("El código del rol es obligatorio.")
                .MaximumLength(50).WithMessage("El código del rol no puede exceder 50 caracteres.");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre del rol es obligatorio.")
                .MaximumLength(80).WithMessage("El nombre del rol no puede exceder 80 caracteres.");

            RuleFor(x => x.Descripcion)
                .MaximumLength(250).WithMessage("La descripción no puede exceder 250 caracteres.");
        }
    }
}
