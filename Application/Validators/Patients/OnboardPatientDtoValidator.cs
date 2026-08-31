using Application.DTOs.Patients;
using FluentValidation;

namespace Application.Validators.Patients;

public sealed class OnboardPatientDtoValidator : AbstractValidator<OnboardPatientDto>
{
    public OnboardPatientDtoValidator()
    {
        RuleFor(x => x.DocumentTypeId)
            .NotEmpty()
            .WithMessage("Document type is required.");

        RuleFor(x => x.DocumentNumber)
            .NotEmpty()
            .WithMessage("Document number is required.")
            .MaximumLength(30)
            .WithMessage("Document number cannot exceed 30 characters.");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Email format is invalid.")
            .MaximumLength(150);

        RuleFor(x => x.Phone)
            .MaximumLength(30);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters.");
    }
}
