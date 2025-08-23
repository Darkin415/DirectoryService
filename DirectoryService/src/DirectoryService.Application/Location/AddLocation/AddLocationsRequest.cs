using DirectoryService.Contacts.Dtos;

namespace DirectoryService.Application.Location.AddLocation;

public record AddLocationsRequest(string Name, AddressDto Address, string TimeZone);
