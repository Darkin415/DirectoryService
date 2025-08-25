using DirectoryService.Contacts.Requests;

namespace DirectoryService.Application.Department;

public record CreateDepartmentCommand(string Name, string Identifier, Guid? ParentId,  List<Guid> Locations);
