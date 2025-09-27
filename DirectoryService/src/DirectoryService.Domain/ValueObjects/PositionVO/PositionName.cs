using CSharpFunctionalExtensions;
using DirectoryService.Contracts.Errors;

namespace DirectoryService.Domain.ValueObjects.PositionVO;

public class PositionName : ValueObject
{
    public string Value { get; }

    public PositionName(string value)
    {
        Value = value;
    }

    public static Result<PositionName, IEnumerable<Error>> Create(string value)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(value))
        {
            var error = Errors.General.ValueIsInvalid("Name");
            
            errors.Add(error);
        }

        if (value.Length < Constants.Constants.SOMETHING_MIN_LENGTH || value.Length > Constants.Constants.SOMETHING_MAX_LENGTH)
        {
            var error = Errors.General.ValueIsInvalid("Name");
            errors.Add(error);
            return errors;
        }
        
        return new PositionName(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
