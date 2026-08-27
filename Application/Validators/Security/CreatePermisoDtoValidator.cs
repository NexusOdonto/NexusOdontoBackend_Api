using Application.DTOs.Permisos;
using FluentValidation;

namespace Application.Validators.Security
{
    public class CreatePermisoDtoValidator : AbstractValidator<CreatePermisoDto>
    {
        public CreatePermisoDtoValidator()
        {
            RuleFor(x => x.Modulo)
                .NotEmpty().WithMessage("El módulo es obligatorio.")
                .MaximumLength(80).WithMessage("El nombre del módulo no puede exceder 80 caracteres.");

            RuleFor(x => x.AccionPermisoId)
                .NotEmpty().WithMessage("Debe indicar una acción de permiso válida.");
        }
    }
}
