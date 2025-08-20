using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.ValueObjects.PositionVO;

public class Description : ValueObject
{
    public Description(string value)
    {
        Value = value;
    }
    
    public string Value {get; }
    
    public static Result<Description, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            var error = Error.Create($"Адрес не может быть пустым");
            return error;
        }

        return new Description(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}