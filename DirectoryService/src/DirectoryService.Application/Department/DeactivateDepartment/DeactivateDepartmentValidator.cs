using FluentValidation;

namespace DirectoryService.Application.Department.DeactivateDepartment;

public class DeactivateDepartmentValidator :AbstractValidator<DeactivateDepartmentCommand>
{
    public DeactivateDepartmentValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id cannot be empty");
    }
}