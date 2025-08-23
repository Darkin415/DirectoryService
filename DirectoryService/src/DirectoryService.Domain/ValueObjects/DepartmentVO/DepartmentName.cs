using CSharpFunctionalExtensions;
using DirectoryService.Contacts.Errors;

namespace DirectoryService.Domain.ValueObjects.DepartmentVO;

public class DepartmentName : ValueObject
{
    public string Value { get; }

    public DepartmentName(string value)
    {
        Value = value;
    }

    public static Result<DepartmentName, IEnumerable<Error>> Create(string value)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(value))
        {
            var error =  Errors.General.ValueIsInvalid("Department name");
            
            errors.Add(error);
        }

        if (value.Length < Constants.Constants.SOMETHING_MIN_LENGTH || value.Length > Constants.Constants.SOMETHING_MAX_LENGTH)
        {
            var error = Errors.General.ValueIsInvalid("Department name");
            errors.Add(error);
            return errors;
        }
        
        return new DepartmentName(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}