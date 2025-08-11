using CSharpFunctionalExtensions;
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
    
    public static Result<LocationId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
            return Error.Create("Guid не может быть пустым");
        
        return new LocationId(value);
    }
    
    
    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}