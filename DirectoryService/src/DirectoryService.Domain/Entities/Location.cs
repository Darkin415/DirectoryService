using CSharpFunctionalExtensions;
using DirectoryService.Domain.ValueObjects;
using DirectoryService.Domain.ValueObjects.LocationVO;
using TimeZone = DirectoryService.Domain.ValueObjects.LocationVO.TimeZone;

namespace DirectoryService.Domain.Entities;

public class Location : Entity<LocationId>
{
    private readonly List<DepartmentLocation> _departmentsLocations = [];

    public Location()
    {
        
    }
    
    public Location(
        LocationName name, 
        TimeZone timeZone,
        Address address)
    {
        Id = LocationId.NewLocationId();
        
        Name = name;
        
        TimeZone = timeZone;
        
        IsActive = true;
        
        CreatedAt = DateTime.UtcNow;
        
        UpdatedAt = CreatedAt;
        
        Address = address;
    }
    
    public LocationName Name {get; private set;}
    
    public Address Address {get; private set;}
    
    public TimeZone TimeZone { get; private set; }
    
    public bool IsActive {get; private set;}
    
    public DateTime CreatedAt {get; private set;}
    
    public DateTime UpdatedAt {get; private set;}
    
    public IReadOnlyList<DepartmentLocation> DepartmentLocations => _departmentsLocations;
    
}