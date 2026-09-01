using Application.Contracts.Repositories.People;
using Application.DTOs.Employees;
using Application.Services.People;
using FluentValidation;

namespace Application.Validators.People;

public class CreateEmployeeDtoValidator : AbstractValidator<CreateEmployeeDto>
{
    public CreateEmployeeDtoValidator(IEmployeeRepository employeeRepository)
    {
        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("Debe indicar una persona válida.");

        RuleFor(x => x.JobTitleId)
            .NotEmpty().WithMessage("Debe indicar un cargo válido.");

        When(x => !string.IsNullOrWhiteSpace(x.EmployeeCode), () =>
        {
            RuleFor(x => x.EmployeeCode!)
                .Must(code => EmployeeCodeGenerator.IsValidEmployeeCodeFormat(code!))
                .WithMessage("Employee code must match format: 2-3 letters followed by 3 digits (e.g. OD001).")
                .MustAsync(async (code, cancellation) =>
                {
                    var existing = await employeeRepository.GetByEmployeeCodeAsync(
                        EmployeeCodeGenerator.NormalizeEmployeeCode(code!),
                        cancellation);
                    return existing is null;
                })
                .WithMessage("Employee code is already in use.");
        });
    }
}
