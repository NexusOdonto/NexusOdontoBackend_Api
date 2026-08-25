using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public sealed class CrearPacienteRequestDtoValidator : AbstractValidator<CrearPacienteRequestDto>
{
    public CrearPacienteRequestDtoValidator()
    {
        RuleFor(x => x.Nombres)
            .NotEmpty()
            .WithMessage("Los nombres son obligatorios.")
            .MaximumLength(100)
            .WithMessage("Los nombres no pueden superar 100 caracteres.");

        RuleFor(x => x.Apellidos)
            .NotEmpty()
            .WithMessage("Los apellidos son obligatorios.")
            .MaximumLength(100)
            .WithMessage("Los apellidos no pueden superar 100 caracteres.");

        RuleFor(x => x.FechaNacimiento)
            .Must(fecha => fecha!.Value.Date <= DateTime.Today)
            .When(x => x.FechaNacimiento.HasValue)
            .WithMessage("La fecha de nacimiento no puede ser futura.");
    }
}
