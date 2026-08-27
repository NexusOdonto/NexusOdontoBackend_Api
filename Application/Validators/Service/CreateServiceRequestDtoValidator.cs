using Application.DTOs;
using FluentValidation;

namespace Application.Validators.Service
{
    public class CreateServiceRequestDtoValidator : AbstractValidator<CreateServiceRequestDto>
    {
        public CreateServiceRequestDtoValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("El código del servicio es obligatorio.")
                .MaximumLength(30).WithMessage("El código no puede exceder los 30 caracteres.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del servicio es obligatorio.")
                .MaximumLength(150).WithMessage("El nombre no puede exceder los 150 caracteres.");

            RuleFor(x => x.DurationMinutes)
                .GreaterThan(0).WithMessage("La duración en minutos debe ser mayor a cero.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).When(x => x.Price.HasValue)
                .WithMessage("El precio no puede ser negativo.");
        }
    }
}
