using CSharpFunctionalExtensions;
using DirectoryService.Contracts.Errors;
using DirectoryService.Domain.ValueObjects.LocationVO;

namespace DirectoryService.Domain.ValueObjects.PositionVO;

public class PositionId : ComparableValueObject
{
    public Guid Value { get; }

    public PositionId(Guid value)
    {
        Value = value;
    }
    
    public static PositionId NewPositionId() => new PositionId(Guid.NewGuid()); 
    
    public static Result<PositionId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
            return Errors.General.ValueIsInvalid("Position");
        
        return new PositionId(value);
    }
    
    
    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}