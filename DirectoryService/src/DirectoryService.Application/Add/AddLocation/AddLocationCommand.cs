using DirectoryService.Contacts.Dtos;

namespace DirectoryService.Application.Add.AddLocation;

public record AddLocationCommand(string Name, AddressDto Address, string TimeZone);
