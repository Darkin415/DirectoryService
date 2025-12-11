using CSharpFunctionalExtensions;
using DirectoryService.Contracts.Errors;

namespace DirectoryService.Domain.ValueObjects.DepartmentVO;

public class Identifier : ValueObject
{
    public Identifier(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public static Result<Identifier, IEnumerable<Error>> Create(string value)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(value))
        {
            var error = Errors.General.ValueIsInvalid("Identifier");
            errors.Add(error);
        }

        if (value.Length < Constants.Constants.SOMETHING_MIN_LENGTH ||
            value.Length > Constants.Constants.SOMETHING_MAX_LENGTH)
        {
            var error = Errors.General.ValueIsInvalid("Identifier");
            errors.Add(error);
            
            return errors;
        }

        return new Identifier(value);
    }

    public static Identifier CreateDeleted(Identifier identifier)
    {
        return new Identifier("deleted" + identifier.Value);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}