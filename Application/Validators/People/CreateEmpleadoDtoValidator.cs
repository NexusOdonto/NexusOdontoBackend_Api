using Application.DTOs.Empleados;
using FluentValidation;

namespace Application.Validators.People
{
    public class CreateEmpleadoDtoValidator : AbstractValidator<CreateEmpleadoDto>
    {
        public CreateEmpleadoDtoValidator()
        {
            RuleFor(x => x.PersonaId)
                .NotEmpty().WithMessage("Debe indicar una persona válida.");

            RuleFor(x => x.CargoId)
                .NotEmpty().WithMessage("Debe indicar un cargo válido.");
        }
    }
}
