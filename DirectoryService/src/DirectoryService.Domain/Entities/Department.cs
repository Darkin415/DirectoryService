using CSharpFunctionalExtensions;
using DirectoryService.Contracts.Errors;
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
        int depth,
        DepartmentId? parentId)
    {
        Id = id;
        
        Name = name;
        
        IsActive = true;
        
        CreatedAt = DateTime.UtcNow;
        
        ChildrenCount = ChildrenDepartments.Count;
        
        UpdatedAt =  DateTime.UtcNow;
        
        Path = path;
        
        Identifier = identifier;
        
        Depth = depth;
        
        ParentId = parentId;

        DeletedAt = null;
    }
    
    public DepartmentId Id { get; private set; }
    
    public DepartmentName Name { get; private set; } 
    
    public int Depth { get; private set; }
    
    public Identifier Identifier { get; private set; } 
    
    public DepartmentId? ParentId { get; private set; }
    
    public Path Path  { get; private set; }
    
    public int ChildrenCount { get; private set; } 
    
    public IReadOnlyList<Department> ChildrenDepartments => _departments;
    
    public IReadOnlyList<DepartmentLocation> DepartmentLocations => _departmentLocations;
    
    public IReadOnlyList<DepartmentPosition> DepartmentPositions => _departmentPositions;
    
    public bool IsActive { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }
    
    public DateTime? DeletedAt { get; private set; }

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
            0,
            null
        );
    }

    public static Result<Department, Error> CreateChild(
        DepartmentName name,
        Identifier identifier,
        Department parent,
        DepartmentId? departmentId = null)
    {
        
        var path = parent.Path.CreateChild(identifier);
        
        parent.ChildrenCount += 1;
        
        return new Department(departmentId ?? new DepartmentId(
            Guid.NewGuid()),
            name, 
            identifier, 
            path,
            parent.Depth + 1, 
            parent.Id);
    }

    public UnitResult<Error> SetParent(Department parent)
    {
        if (parent == this)
            return Error.Conflict("self.department", "Can not set yourself");

        var newPath = Path.CalculateNewPath(parent.Path, Identifier.Value);
        if (newPath.IsFailure)
            return newPath.Error;

        int newDepth = 1;
        if (parent != null)
            newDepth = parent.Depth + 1;

        ParentId = parent?.Id;
        Depth = newDepth;
        Path = newPath.Value;
        
        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> AddDepartmentLocations(List<DepartmentLocation> departmentLocations)
    {
        _departmentLocations.AddRange(departmentLocations);
        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> UpdateDepartmentLocations(List<DepartmentLocation> newDepartmentLocations)
    {
        _departmentLocations.Clear();
        
        _departmentLocations.AddRange(newDepartmentLocations);
        
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> Deactivate()
    {
        IsActive = false;
        DeletedAt = DateTime.UtcNow;

        var oldIdentifier = Identifier;
        
        Identifier = Identifier.CreateDeleted(Identifier);
        
        var newPath = Path.CreateDeleted(oldIdentifier.Value, Path);
        
        Path = newPath;
        
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> Activate()
    {
        var oldDeletedAt = DeletedAt;
        
        var oldIdentifier = Identifier;

        IsActive = true;
        DeletedAt = null;

        var activeIdentifierResult = Identifier.Create(Identifier.Value.Replace("Deleted", ""));
        
        if (activeIdentifierResult.IsFailure)
        {
            IsActive = false;
            DeletedAt = oldDeletedAt;
            return UnitResult.Failure<Error>(activeIdentifierResult.Error.First()); 
        }
        
        Identifier = activeIdentifierResult.Value;
        var newPathResult = Path.Create(Path.Value.Replace(oldIdentifier.Value, Identifier.Value));
        
        if (newPathResult.IsFailure)
        {
            IsActive = false;
            DeletedAt = oldDeletedAt;
            Identifier = oldIdentifier;
            return newPathResult.Error;
        }
        
        Path = newPathResult.Value;
        
        return UnitResult.Success<Error>();
    }
}