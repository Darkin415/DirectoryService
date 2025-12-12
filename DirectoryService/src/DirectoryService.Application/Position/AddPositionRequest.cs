namespace DirectoryService.Application.Position;

public record AddPositionRequest(string Name, string? Description, List<Guid> DepartmentIds);
