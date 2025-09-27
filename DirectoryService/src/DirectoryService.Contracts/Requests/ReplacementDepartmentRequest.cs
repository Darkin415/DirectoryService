namespace DirectoryService.Contracts.Requests;

public record ReplacementDepartmentRequest(Guid ParentId, Guid DepartmentId);
