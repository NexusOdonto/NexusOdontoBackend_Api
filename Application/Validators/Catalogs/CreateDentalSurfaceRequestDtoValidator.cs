using Application.DTOs.Catalogs.DentalSurface;
using FluentValidation;

namespace Application.Validators.Catalogs
{
    public class CreateDentalSurfaceRequestDtoValidator : AbstractValidator<CreateDentalSurfaceRequestDto>
    {
        public CreateDentalSurfaceRequestDtoValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("El código es obligatorio.")
                .MaximumLength(20).WithMessage("El código no puede exceder los 20 caracteres.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(80).WithMessage("El nombre no puede exceder los 80 caracteres.");
        }
    }
}
