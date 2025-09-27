namespace DirectoryService.Contracts.Dtos;

public class DepartmentTopDto
{
    public Guid Id { get; set; }
    
    public string DepartmentName { get; set; } = string.Empty!;
    
    public int PositionCount { get; set; }
}