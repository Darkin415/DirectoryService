using CSharpFunctionalExtensions;
using DirectoryService.Domain.ValueObjects;
using DirectoryService.Domain.ValueObjects.LocationVO;
using TimeZone = DirectoryService.Domain.ValueObjects.LocationVO.TimeZone;

namespace DirectoryService.Domain.Entities;

public class Location : Entity<LocationId>
{
    private readonly List<Address> _addresses = [];
    
    private readonly List<Department> _departments;

    public Location()
    {
        
    }
    
    public Location(
        LocationName name, 
        TimeZone timeZone)
    {
        Id = LocationId.NewLocationId();
        
        Name = name;
        
        TimeZone = timeZone;
        
        IsActive = true;
        
        CreatedAt = DateTime.UtcNow;
        
        UpdatedAt = CreatedAt;
    }
    
    public LocationName Name {get; private set;}
    
    public TimeZone TimeZone { get; private set; }
    
    public bool IsActive {get; private set;}
    
    public DateTime CreatedAt {get; private set;}
    
    public DateTime UpdatedAt {get; private set;}
    
    public IReadOnlyList<Department> Departments => _departments;
    
    public IReadOnlyList<Address> Addresses => _addresses;
    
}