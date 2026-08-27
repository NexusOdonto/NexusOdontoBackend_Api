using Application.DTOs.Catalogs.AntecedentType;
using FluentValidation;

namespace Application.Validators.Catalogs
{
    public class CreateAntecedentTypeRequestDtoValidator : AbstractValidator<CreateAntecedentTypeRequestDto>
    {
        public CreateAntecedentTypeRequestDtoValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("El código es obligatorio.")
                .MaximumLength(20).WithMessage("El código no puede exceder los 20 caracteres.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(80).WithMessage("El nombre no puede exceder los 80 caracteres.");

            RuleFor(x => x.Description)
                .MaximumLength(300).WithMessage("La descripción no puede exceder los 300 caracteres.");
        }
    }
}
