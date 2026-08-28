using Application.DTOs.Roles;
using FluentValidation;

namespace Application.Validators.Security
{
    public class CreateRoleDtoValidator : AbstractValidator<CreateRoleDto>
    {
        public CreateRoleDtoValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("El código del rol es obligatorio.")
                .MaximumLength(50).WithMessage("El código del rol no puede exceder 50 caracteres.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del rol es obligatorio.")
                .MaximumLength(80).WithMessage("El nombre del rol no puede exceder 80 caracteres.");

            RuleFor(x => x.Description)
                .MaximumLength(250).WithMessage("La descripción no puede exceder 250 caracteres.");
        }
    }
}
