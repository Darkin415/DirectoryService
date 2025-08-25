using FluentValidation;

namespace DirectoryService.Application.Department;

public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator()
    {
        RuleFor(x => x.AddDepartmentRequest.Name).Length(3, 150).WithMessage("Invalid Length").NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.AddDepartmentRequest.Identifier).Length(3,150).WithMessage("Invalid Length").NotEmpty().WithMessage("Identifier is required");
        RuleFor(x => x.AddDepartmentRequest.ParentId).NotEmpty().WithMessage("Parent Id is required");
        RuleFor(c => c.AddDepartmentRequest.Locations)
            .Must(locations => locations.Count == locations.Distinct().Count())
            .WithMessage("Locations must unique")
            .NotEmpty().WithMessage("Locations is required");
    }
}