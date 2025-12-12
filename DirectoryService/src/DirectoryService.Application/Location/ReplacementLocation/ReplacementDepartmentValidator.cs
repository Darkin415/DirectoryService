using FluentValidation;

namespace DirectoryService.Application.Location.ReplacementLocation;

public class ReplacementDepartmentValidator : AbstractValidator<ReplacementDepartmentCommand>
{
    public ReplacementDepartmentValidator()
    {
        RuleFor(x => x.ParentId).NotEmpty().WithMessage("Parent Id is required");
        RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("DepartmentId is required");
    }
}