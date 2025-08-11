using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.ValueObjects.LocationVO;

public class TimeZone : ValueObject
{
    public TimeZone(string value)
    {
        Value = value;
    }
    
    public string Value {get;}

    public static Result<TimeZone, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Error.Create("Значение не может быть пустым");

        return new TimeZone(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}