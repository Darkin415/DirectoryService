using DirectoryService.Contracts.Validation;
using DirectoryService.Domain.ValueObjects.LocationVO;
using FluentValidation;
using TimeZone = DirectoryService.Domain.ValueObjects.LocationVO.TimeZone;

namespace DirectoryService.Application.Location.AddLocation;

public class AddLocationCommandValidator : AbstractValidator<AddLocationCommand>
{
    public AddLocationCommandValidator()
    {
        RuleFor(x => x.Name).MustBeValueObject(LocationName.Create);
        RuleFor(x => x.Address).MustBeValueObject(x => 
            Address.Create(x.Country, x.City, x.Street, x.Building, x.RoomNumber));
        RuleFor(x => x.TimeZone).MustBeValueObject(TimeZone.Create);
    }
}