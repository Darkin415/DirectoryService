using DirectoryService.Contacts.Dtos;

namespace DirectoryService.Application.Add.AddLocation;

public record AddLocationsRequest(string Name, AddressDto Address, string TimeZone);
