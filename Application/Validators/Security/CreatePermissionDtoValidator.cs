using Application.DTOs.Permissions;
using FluentValidation;

namespace Application.Validators.Security
{
    public class CreatePermissionDtoValidator : AbstractValidator<CreatePermissionDto>
    {
        public CreatePermissionDtoValidator()
        {
            RuleFor(x => x.Module)
                .NotEmpty().WithMessage("El módulo es obligatorio.")
                .MaximumLength(80).WithMessage("El nombre del módulo no puede exceder 80 caracteres.");

            RuleFor(x => x.ActionPermissionId)
                .NotEmpty().WithMessage("Debe indicar una acción de permiso válida.");
        }
    }
}
