using FluentValidation;

namespace DirectoryService.Application.Position;

public class AddPositionValidator :  AbstractValidator<AddPositionCommand>
{
    public AddPositionValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.DepartmentIds).NotEmpty().WithMessage("DepartmentIds is required");
    }
}