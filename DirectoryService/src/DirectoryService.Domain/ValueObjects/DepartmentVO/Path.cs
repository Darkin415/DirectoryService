using CSharpFunctionalExtensions;
using DirectoryService.Contacts.Errors;

namespace DirectoryService.Domain.ValueObjects.DepartmentVO;

public class Path : ValueObject
{
    private const char Separator = '/';
    public string Value { get; }

    public Path(string value)
    {
        Value = value;
    }

    public static Path CreateParent(Identifier identifier)
    {
        return new Path(identifier.Value);
    }

    public Path CreateChild(Identifier childIdentifier)
    {
        return new Path(Value + Separator + childIdentifier.Value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}