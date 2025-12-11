using DirectoryService.Contracts.Dtos;

namespace DirectoryService.Application.Location.AddLocation;

public record AddLocationCommand(string Name, AddressDto Address, string TimeZone);
