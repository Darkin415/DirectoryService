using CSharpFunctionalExtensions;
using DirectoryService.Contracts.Errors;

namespace DirectoryService.Domain.ValueObjects.DepartmentVO;

public class DepartmentId : ComparableValueObject
{
    public Guid Value { get; }

    public DepartmentId(Guid value)
    {
        Value = value;
    }
    
    public static DepartmentId NewDepartmentId() => new DepartmentId(Guid.NewGuid());
    
    public static Result<DepartmentId, Error> Create(Guid value)
    {
        if (value == Guid.Empty)
            return  Errors.General.ValueIsInvalid("DepartmentId");
        
        return new DepartmentId(value);
    }
    
    
    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
         yield return Value;
    }
}