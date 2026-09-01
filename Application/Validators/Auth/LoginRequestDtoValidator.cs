using Application.DTOs.Auth;
using FluentValidation;

namespace Application.Validators.Auth;

public sealed class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator()
    {
        RuleFor(x => x.LoginId)
            .NotEmpty()
            .WithMessage("Login ID is required.")
            .MaximumLength(30)
            .WithMessage("Login ID cannot exceed 30 characters.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters.");
    }
}
