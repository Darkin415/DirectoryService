using DirectoryService.Contracts.Requests;

namespace DirectoryService.Application.Department;

public record CreateDepartmentCommand(string Name, string Identifier, Guid? ParentId,  List<Guid> LocationsIds);
