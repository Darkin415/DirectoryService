using FluentValidation;

namespace DirectoryService.Application.Location.UpdateLocation;

public class UpdateLocationCommandValidator : AbstractValidator<UpdateLocationCommand>
{
    public UpdateLocationCommandValidator()
    {
        RuleFor(command => command.LocationIds).NotEmpty().WithMessage("Please specify location ids");
    }
}