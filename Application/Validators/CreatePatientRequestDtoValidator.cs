using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public sealed class CreatePatientRequestDtoValidator : AbstractValidator<CreatePatientRequestDto>
{
    public CreatePatientRequestDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MaximumLength(100)
            .WithMessage("First name cannot exceed 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MaximumLength(100)
            .WithMessage("Last name cannot exceed 100 characters.");

        RuleFor(x => x.DateOfBirth)
            .Must(date => date!.Value.Date <= DateTime.Today)
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("Date of birth cannot be in the future.");
    }
}
