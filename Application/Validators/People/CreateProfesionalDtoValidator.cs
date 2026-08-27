using Application.DTOs.Profesionales;
using FluentValidation;

namespace Application.Validators.People
{
    public class CreateProfesionalDtoValidator : AbstractValidator<CreateProfesionalDto>
    {
        public CreateProfesionalDtoValidator()
        {
            RuleFor(x => x.EmpleadoId)
                .NotEmpty().WithMessage("Debe indicar un empleado válido.");

            RuleFor(x => x.RegistroProfesional)
                .NotEmpty().WithMessage("El registro profesional es obligatorio.")
                .MaximumLength(50).WithMessage("El registro profesional no puede exceder 50 caracteres.");
        }
    }
}
