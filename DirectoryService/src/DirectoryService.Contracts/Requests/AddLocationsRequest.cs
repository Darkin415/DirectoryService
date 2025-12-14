using DirectoryService.Contracts.Dtos;

namespace DirectoryService.Contracts.Requests;

public record AddLocationsRequest(string Name, AddressDto Address, string TimeZone);
 