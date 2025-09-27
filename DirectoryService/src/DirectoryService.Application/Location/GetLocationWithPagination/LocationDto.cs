using DirectoryService.Contracts.Dtos;
using DirectoryService.Domain.Entities;
using TimeZone = DirectoryService.Domain.ValueObjects.LocationVO.TimeZone;

namespace DirectoryService.Application.Location.GetLocationWithPagination;

public record LocationDto
{
    public string Name { get; init; } = string.Empty;
    
    public string Country { get; set; } = string.Empty;
    
    public string City { get; set; } = string.Empty;
    
    public string Street { get; set; } = string.Empty;
    
    public string Building { get; set; } = string.Empty;
    
    public int RoomNumber { get; set; }
    
    public TimeZone TimeZone { get; init; }
    
    public bool IsActive {get; init;}
    
    public DateTime CreatedAt {get; init;}
    
    public DateTime UpdatedAt {get; init;}
    
};

public record GetLocationDto(List<LocationDto> Locations, long TotalCount);
