using CSharpFunctionalExtensions;
using DirectoryService.Contacts.Errors;

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
            var error = Errors.General.ValueIsInvalid("Description");
            return error;
        }

        return new Description(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}