namespace DirectoryService.Application.Location.UpdateLocation;

public record UpdateLocationCommand(Guid DepartmentId, List<Guid> LocationIds);
