using Application.DTOs.Auth;
using FluentValidation;

namespace Application.Validators.Auth;

public sealed class GoogleLinkRequestDtoValidator : AbstractValidator<GoogleLinkRequestDto>
{
    public GoogleLinkRequestDtoValidator()
    {
        RuleFor(x => x.LinkToken)
            .NotEmpty()
            .WithMessage("Link token is required.");

        RuleFor(x => x.DocumentNumber)
            .NotEmpty()
            .WithMessage("Document number is required.")
            .MaximumLength(30)
            .WithMessage("Document number cannot exceed 30 characters.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters.");
    }
}
