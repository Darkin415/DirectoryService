using CSharpFunctionalExtensions;

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
            return Error.Create("Guid не может быть пустым");
        
        return new DepartmentId(value);
    }
    
    
    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
         yield return Value;
    }
}