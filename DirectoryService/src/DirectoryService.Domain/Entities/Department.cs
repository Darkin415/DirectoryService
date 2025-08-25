using CSharpFunctionalExtensions;
using DirectoryService.Contacts.Errors;
using DirectoryService.Domain.ValueObjects;
using DirectoryService.Domain.ValueObjects.DepartmentVO;
using Path = DirectoryService.Domain.ValueObjects.DepartmentVO.Path;

namespace DirectoryService.Domain.Entities;

public class Department : Entity<DepartmentId>
{
    private readonly List<DepartmentLocation> _departmentLocations = [];
    private readonly List<DepartmentPosition> _departmentPositions = [];
    private readonly List<Department> _departments = [];
    
    public Department()
    {
        
    }
    
    private Department(
        DepartmentId id,
        DepartmentName name, 
        Identifier identifier,
        Path path,
        int depth)
    {
        Id = id;
        
        Name = name;
        
        IsActive = true;
        
        CreatedAt = DateTime.UtcNow;
        
        ChildrenCount = ChildrenDepartments.Count;
        
        UpdatedAt =  DateTime.UtcNow;
        
        Path = path;
        
        Depth = depth;
    }
    
    public DepartmentId Id { get; private set; }
    
    public DepartmentName Name { get; private set; } 
    
    public int Depth { get; private set; }
    
    public string Identifier { get; private set; } = string.Empty;
    
    public DepartmentId ParentId { get; private set; }
    
    public Path Path  { get; private set; }
    
    public int ChildrenCount { get; private set; } 
    
    public IReadOnlyList<Department> ChildrenDepartments => _departments;
    
    public IReadOnlyList<DepartmentLocation> DepartmentLocations => _departmentLocations;
    
    public IReadOnlyList<DepartmentPosition> DepartmentPositions => _departmentPositions;
    
    public bool IsActive { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }

    public static Result<Department, Error> CreateParent(
        DepartmentName name,
        Identifier identifier,
        DepartmentId? departmentId = null)
    {
        var path = ValueObjects.DepartmentVO.Path.CreateParent(identifier);
        
        return new Department(
            departmentId ?? new DepartmentId(Guid.NewGuid()),
            name,
            identifier,            
            path,
            0
        );
    }

    public static Result<Department, Error> CreateChild(
        DepartmentName name,
        Identifier identifier,
        Department parent,
        DepartmentId? departmentId = null)
    {
        
        var path = parent.Path.CreateChild(identifier);
        
        return new Department(departmentId ?? new DepartmentId(Guid.NewGuid()), name, identifier, path, parent.Depth + 1);
    }
    
    public UnitResult<Error> AddDepartmentLocations(List<DepartmentLocation> departmentLocations)
    {
        _departmentLocations.AddRange(departmentLocations);
        return UnitResult.Success<Error>();
    }
}