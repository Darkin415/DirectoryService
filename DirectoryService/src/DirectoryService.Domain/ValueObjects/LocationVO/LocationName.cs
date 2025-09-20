using CSharpFunctionalExtensions;
using DirectoryService.Contracts.Errors;

namespace DirectoryService.Domain.ValueObjects.LocationVO;

public class LocationName : ValueObject
{
    public string Value { get; }

    public LocationName(string value)
    {
        Value = value;
    }

    public static Result<LocationName, Error> Create(string value)
    {

        if (string.IsNullOrWhiteSpace(value))
        {
            return Errors.General.ValueIsInvalid();
        }

        if (value.Length < Constants.Constants.SOMETHING_MIN_LENGTH || value.Length > Constants.Constants.SOMETHING_MAX_LENGTH)
        {
            return Errors.General.ValueIsInvalid();
        }
        
        return new LocationName(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}