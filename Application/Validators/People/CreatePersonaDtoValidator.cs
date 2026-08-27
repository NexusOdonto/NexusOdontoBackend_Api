using System;
using Application.DTOs.Personas;
using FluentValidation;

namespace Application.Validators.People
{
    public class CreatePersonaDtoValidator : AbstractValidator<CreatePersonaDto>
    {
        public CreatePersonaDtoValidator()
        {
            RuleFor(x => x.TipoDocumentoId)
                .NotEmpty().WithMessage("Debe especificar un tipo de documento válido.");

            RuleFor(x => x.NumeroDocumento)
                .NotEmpty().WithMessage("El número de documento es obligatorio.")
                .MaximumLength(30).WithMessage("El número de documento no puede exceder 30 caracteres.");

            RuleFor(x => x.Nombres)
                .NotEmpty().WithMessage("Los nombres son obligatorios.")
                .MaximumLength(100).WithMessage("Los nombres no pueden exceder 100 caracteres.");

            RuleFor(x => x.Apellidos)
                .NotEmpty().WithMessage("Los apellidos son obligatorios.")
                .MaximumLength(100).WithMessage("Los apellidos no pueden exceder 100 caracteres.");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("El formato del correo electrónico no es válido.")
                .MaximumLength(150).WithMessage("El correo electrónico no puede exceder 150 caracteres.");

            RuleFor(x => x.Telefono)
                .MaximumLength(30).WithMessage("El teléfono no puede exceder 30 caracteres.");
        }
    }
}
