using DirectoryService.Contacts.Dtos;

namespace DirectoryService.Contacts.Requests;

public record AddLocationsRequest(string Name, AddressDto Address, string TimeZone);
