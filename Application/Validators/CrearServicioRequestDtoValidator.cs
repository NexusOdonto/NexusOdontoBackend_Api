using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public sealed class CrearServicioRequestDtoValidator : AbstractValidator<CrearServicioRequestDto>
{
    public CrearServicioRequestDtoValidator()
    {
        RuleFor(x => x.DuracionMinutos)
            .GreaterThan(0)
            .WithMessage("La duracion en minutos debe ser mayor que cero.");

        RuleFor(x => x.Precio)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Precio.HasValue)
            .WithMessage("El precio no puede ser negativo.");
    }
}
