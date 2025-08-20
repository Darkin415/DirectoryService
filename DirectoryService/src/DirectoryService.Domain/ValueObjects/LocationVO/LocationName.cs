using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.ValueObjects.LocationVO;

public class LocationName : ValueObject
{
    public string Value { get; }

    public LocationName(string value)
    {
        Value = value;
    }

    public static Result<LocationName, IEnumerable<Error>> Create(string value)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(value))
        {
            var error = Error.Create("Имя департамента не может быть пустым");
            
            errors.Add(error);
        }

        if (value.Length < Constants.Constants.SOMETHING_MIN_LENGTH || value.Length > Constants.Constants.SOMETHING_MAX_LENGTH)
        {
            var error = Error.Create("Недопустимый размер имени департамента");
            errors.Add(error);
            return errors;
        }
        
        return new LocationName(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}