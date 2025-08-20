using CSharpFunctionalExtensions;

namespace DirectoryService.Domain.ValueObjects.LocationVO;

public class Address : ValueObject
{
    public Address(string value)
    {
        Value = value;
    }
    
    public string Value {get; }
    
    public static Result<Address, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            var error = Error.Create($"Адрес не может быть пустым");
            return error;
        }

        return new Address(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}