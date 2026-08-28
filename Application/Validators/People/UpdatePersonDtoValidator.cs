using Application.DTOs.People;
using FluentValidation;

namespace Application.Validators.People
{
    public class UpdatePersonDtoValidator : AbstractValidator<UpdatePersonDto>
    {
        public UpdatePersonDtoValidator()
        {
            RuleFor(x => x.DocumentTypeId)
                .NotEmpty().WithMessage("Debe especificar un tipo de documento válido.");

            RuleFor(x => x.DocumentNumber)
                .NotEmpty().WithMessage("El número de documento es obligatorio.")
                .MaximumLength(30).WithMessage("El número de documento no puede exceder 30 caracteres.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Los nombres son obligatorios.")
                .MaximumLength(100).WithMessage("Los nombres no pueden exceder 100 caracteres.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Los apellidos son obligatorios.")
                .MaximumLength(100).WithMessage("Los apellidos no pueden exceder 100 caracteres.");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("El formato del correo electrónico no es válido.");
        }
    }
}
