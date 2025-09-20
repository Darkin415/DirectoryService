using CSharpFunctionalExtensions;
using DirectoryService.Contracts.Errors;
using DirectoryService.Domain.ValueObjects;
using DirectoryService.Domain.ValueObjects.PositionVO;

namespace DirectoryService.Domain.Entities;

public class Position : Entity<PositionId>
{
    private readonly List<DepartmentPosition> _departmentPositions = [];


    public Position()
    {
        
    }
    
    public Position(PositionName name, Description? description)
    {
        Id = PositionId.NewPositionId();
        
        Name = name;
        
        Description = description;
        
        CreatedAt = DateTime.UtcNow;
        
        UpdatedAt = CreatedAt;
        
        IsActive = true;
        
    }
    
    public PositionName Name {get; private set;}
    
    public Description Description {get; private set;}
    
    public bool IsActive {get; private set;}
    
    public DateTime CreatedAt {get; private set;}
    
    public DateTime UpdatedAt {get; private set;}

    public IReadOnlyList<DepartmentPosition> DepartmentPositions => _departmentPositions;
    
    public UnitResult<Error> AddDepartmentPositions(List<DepartmentPosition> departmentPositions)
    {
        _departmentPositions.AddRange(departmentPositions);
        return UnitResult.Success<Error>();
    }
}