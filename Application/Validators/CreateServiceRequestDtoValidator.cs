using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public sealed class CreateServiceRequestDtoValidator : AbstractValidator<CreateServiceRequestDto>
{
    public CreateServiceRequestDtoValidator()
    {
        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0)
            .WithMessage("Duration in minutes must be greater than zero.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Price.HasValue)
            .WithMessage("Price cannot be negative.");
    }
}
