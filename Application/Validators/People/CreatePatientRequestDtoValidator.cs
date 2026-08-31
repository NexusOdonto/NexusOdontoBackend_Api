using System;
using Application.DTOs;
using FluentValidation;

namespace Application.Validators.People
{
    public class CreatePatientRequestDtoValidator : AbstractValidator<CreatePatientRequestDto>
    {
        public CreatePatientRequestDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El apellido es obligatorio.")
                .MaximumLength(100).WithMessage("El apellido no puede exceder 100 caracteres.");

            RuleFor(x => x.DateOfBirth)
                .Must(date => !date.HasValue || date.Value <= DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("La fecha de nacimiento no puede estar en el futuro.");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("El formato del correo electrónico no es válido.");
        }
    }
}
