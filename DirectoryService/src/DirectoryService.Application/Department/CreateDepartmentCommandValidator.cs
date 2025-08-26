using FluentValidation;

namespace DirectoryService.Application.Department;

public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator()
    {
        RuleFor(x => x.Name).Length(3, 150).WithMessage("Invalid Length").NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Identifier).Length(3,150).WithMessage("Invalid Length").NotEmpty().WithMessage("Identifier is required");
        RuleFor(c => c.LocationsIds)
            .Must(locations => locations.Count == locations.Distinct().Count())
            .WithMessage("Locations must unique")
            .NotEmpty().WithMessage("Locations is required");
    }
}