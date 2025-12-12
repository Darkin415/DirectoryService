using CSharpFunctionalExtensions;
using DirectoryService.Contracts.Errors;

namespace DirectoryService.Domain.ValueObjects.LocationVO;

public class LocationId : ComparableValueObject
{
    public Guid Value { get; }

    public LocationId(Guid value)
    {
        Value = value;
    }
    
    public static LocationId NewLocationId() => new LocationId(Guid.NewGuid()); 
    
    public static Result<LocationId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
            return Errors.General.ValueIsInvalid("LocationId");
        
        return new LocationId(value);
    }
    
    
    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}