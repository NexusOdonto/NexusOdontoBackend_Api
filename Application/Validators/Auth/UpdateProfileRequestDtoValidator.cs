using Application.DTOs.Auth;
using FluentValidation;

namespace Application.Validators.Auth;

public sealed class UpdateProfileRequestDtoValidator : AbstractValidator<UpdateProfileRequestDto>
{
    public UpdateProfileRequestDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Los nombres son obligatorios.")
            .MaximumLength(100).WithMessage("Los nombres no pueden exceder 100 caracteres.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Los apellidos son obligatorios.")
            .MaximumLength(100).WithMessage("Los apellidos no pueden exceder 100 caracteres.");

        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("El formato del correo electrónico no es válido.");

        RuleFor(x => x.DocumentNumber)
            .MaximumLength(30).When(x => !string.IsNullOrWhiteSpace(x.DocumentNumber))
            .WithMessage("El número de documento no puede exceder 30 caracteres.");

        RuleFor(x => x.Phone)
            .MaximumLength(30).When(x => !string.IsNullOrWhiteSpace(x.Phone))
            .WithMessage("El teléfono no puede exceder 30 caracteres.");

        RuleFor(x => x.Address)
            .MaximumLength(250).When(x => !string.IsNullOrWhiteSpace(x.Address))
            .WithMessage("La dirección no puede exceder 250 caracteres.");
    }
}
