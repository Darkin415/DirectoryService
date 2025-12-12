namespace DirectoryService.Application.Department.Query.GetChildDepartments;

public record GetChildDepartmentRequest(Guid ParentId, int Page, int PageSize);
