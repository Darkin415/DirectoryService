using CSharpFunctionalExtensions;
using DirectoryService.Domain.ValueObjects;
using DirectoryService.Domain.ValueObjects.DepartmentVO;
using Path = DirectoryService.Domain.ValueObjects.DepartmentVO.Path;

namespace DirectoryService.Domain.Entities;

public class Department : Entity<DepartmentId>
{
    private readonly List<Department> _departments = [];
    
    private readonly List<Location> _locations = [];
    
    private readonly List<Position> _positions = [];

    public Department()
    {
        
    }
    
    public Department(
        DepartmentName name, 
        Identifier identifier,
        Path path)
    {
        Id = DepartmentId.NewDepartmentId();
        
        Name = name;
        
        IsActive = true;
        
        CreatedAt = DateTime.UtcNow;
        
        UpdatedAt = CreatedAt;
        
        Path = path;
    }
    
    public DepartmentName Name { get; private set; } 
    
    public string Identifier { get; private set; } = string.Empty;
    
    public DepartmentId ParentId { get; private set; }
    
    public Path Path  { get; private set; }
    
    public int ChildrenCount { get; private set; } 
    
    public IReadOnlyList<Department> ChildrenDepartments => _departments;
    
    public IReadOnlyList<Location> Locations => _locations;

    public IReadOnlyList<Position> Positions => _positions;
    
    public bool IsActive { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }
  
}