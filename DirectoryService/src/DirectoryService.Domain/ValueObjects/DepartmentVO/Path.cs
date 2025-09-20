using CSharpFunctionalExtensions;
using DirectoryService.Contracts.Errors;

namespace DirectoryService.Domain.ValueObjects.DepartmentVO;

public class Path : ValueObject
{
    private const char Separator = '.';
    public string Value { get; }

    public Path(string value)
    {
        Value = value;
    }

    public static Path CreateParent(Identifier identifier)
    {
        return new Path(identifier.Value);
    }

    public Result<Path,Error> CalculateNewPath(Path? newParentPath, string identifier)
    {
        if(newParentPath == null)
            return new Path(identifier);
        
        return new Path(newParentPath.Value + Separator + identifier);
    }

    public static Result<Path,Error> Create(string value)
    {
        if (String.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsInvalid("Value");
        
        return new Path(value);
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