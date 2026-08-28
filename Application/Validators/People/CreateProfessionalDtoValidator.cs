using Application.DTOs.Professionals;
using FluentValidation;

namespace Application.Validators.People
{
    public class CreateProfessionalDtoValidator : AbstractValidator<CreateProfessionalDto>
    {
        public CreateProfessionalDtoValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("Debe indicar un empleado válido.");

            RuleFor(x => x.ProfessionalLicense)
                .NotEmpty().WithMessage("El registro profesional es obligatorio.")
                .MaximumLength(50).WithMessage("El registro profesional no puede exceder 50 caracteres.");
        }
    }
}
