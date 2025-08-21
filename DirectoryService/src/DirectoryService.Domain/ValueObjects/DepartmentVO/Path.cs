using CSharpFunctionalExtensions;
using DirectoryService.Contacts.Errors;

namespace DirectoryService.Domain.ValueObjects.DepartmentVO;

public class Path : ValueObject
{
    public string Value { get; }

    public Path(string value)
    {
        Value = value;
    }

    public static Result<Path, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            Errors.General.ValueIsInvalid("Path");

        return new Path(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}