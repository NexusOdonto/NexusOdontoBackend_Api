using Application.DTOs.Employees;
using FluentValidation;

namespace Application.Validators.People
{
    public class CreateEmployeeDtoValidator : AbstractValidator<CreateEmployeeDto>
    {
        public CreateEmployeeDtoValidator()
        {
            RuleFor(x => x.PersonId)
                .NotEmpty().WithMessage("Debe indicar una persona válida.");

            RuleFor(x => x.JobTitleId)
                .NotEmpty().WithMessage("Debe indicar un cargo válido.");
        }
    }
}
