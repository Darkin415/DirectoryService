using CSharpFunctionalExtensions;
using DirectoryService.Contacts.Errors;

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
            return Errors.General.ValueIsInvalid("Значение не может быть пустым");
        
        var isValid = TimeZoneInfo.TryFindSystemTimeZoneById(value, out var _);
        if (isValid == false)
            return Errors.General.ValueIsInvalid($"Временная зона с идентификатором '{value}' не найдена");

        return new TimeZone(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}