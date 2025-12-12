using System.Data;
using Dapper;
using DirectoryService.Application.Interfaces;
using DirectoryService.Contracts.Dtos;


namespace DirectoryService.Application.Department.Query.GetRootDepartment;

public class GetRootDepartmentHandler
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IDbConnection _connection;

    public GetRootDepartmentHandler(ISqlConnectionFactory sqlConnectionFactory, IDbConnection connection)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _connection = connection;
    }

    public async Task<List<DepartmentDto>> GetRootDepartments(GetRootDepartmentRequest request, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.Create();
        
        const string sql = @"
        WITH roots AS (
            SELECT
                d.id,
                d.""parentId"",
                d.department_name,
                d.department_identifier,
                d.path,
                d.depth,
                d.is_active,
                d.created_at,
                d.updated_at
            FROM department.departments d
            WHERE d.""parentId"" IS NULL
            ORDER BY d.created_at
            LIMIT @RootsLimit  OFFSET @RootsOffset
        )
        SELECT *, 
               (EXISTS(SELECT 1 FROM department.departments d WHERE d.""parentId"" = roots.id OFFSET @ChildrenLimit LIMIT 1)) as HasMoreChildren 
        FROM roots

        UNION ALL

        SELECT c.*, 
                 (EXISTS(SELECT 1 FROM department.departments d WHERE d.""parentId"" = c.id)) as HasMoreChildren 
        FROM roots r 
        CROSS JOIN LATERAL (
            SELECT
                d.id,
                d.""parentId"",
                d.department_name,
                d.department_identifier,
                d.path,
                d.depth,
                d.is_active,
                d.created_at,
                d.updated_at
            FROM department.departments d
            WHERE d.""parentId"" = r.id AND d.is_active = TRUE
            LIMIT @ChildrenLimit 
        ) c;
    ";
        
        var flatResult = await connection.QueryAsync<DepartmentDto>(
            sql,
            new
            {
                RootsLimit = request.PageSize,
                RootsOffset = (request.Page - 1) * request.PageSize,
                ChildrenLimit = request.Prefetch,
            });
        
        var departmentWithChildrenDtos = flatResult.ToList();
        
        var roots = departmentWithChildrenDtos
            .Where(dep => dep.ParentId == null)
            .ToList();
        
        var childrenDict = departmentWithChildrenDtos
            .Where(dep => dep.ParentId != null)
            .GroupBy(dep => dep.ParentId)
            .ToDictionary(g => g.Key, g => g.ToList());
        

        foreach (var root in roots)
        {
            if (childrenDict.TryGetValue(root.Id, out var children))
                root.Children.AddRange(children);
        }
        
        return roots;
    }
    
}