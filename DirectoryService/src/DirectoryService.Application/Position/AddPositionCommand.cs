namespace DirectoryService.Application.Position;

public record AddPositionCommand(string Name, string? Description, List<Guid> DepartmentIds);
