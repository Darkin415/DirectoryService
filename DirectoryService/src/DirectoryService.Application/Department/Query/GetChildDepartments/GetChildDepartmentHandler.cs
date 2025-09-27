using System.Data;
using Dapper;
using DirectoryService.Application.Interfaces;
using DirectoryService.Contracts.Dtos;

namespace DirectoryService.Application.Department.Query.GetChildDepartments;

public class GetChildDepartmentHandler
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IDbConnection _connection;

    public GetChildDepartmentHandler(IDbConnection connection, ISqlConnectionFactory sqlConnectionFactory)
    {
        _connection = connection;
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<List<DepartmentDto>> GetChildDepartment(GetChildDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.Create();

        const string sql = @"
            SELECT
             d.id,
             d.""parentId"",
             d.department_name,
             d.department_identifier,
             d.path,
             d.depth,
             d.is_active,
             d.created_at,
             d.updated_at,
            (EXISTS(SELECT 1 FROM department.departments child WHERE child.""parentId"" = d.id LIMIT 1)) as HasMoreChildren 
        FROM department.departments d
        WHERE d.""parentId"" = @ParentId
        LIMIT @Limit
        OFFSET @Offset
    ";

        var flatResult = await connection.QueryAsync<DepartmentDto>(
            sql,
            new
            {
                ParentId = request.ParentId,
                Limit = request.PageSize,
                Offset = (request.Page - 1) * request.PageSize
            });

        var departmentWithChildrenDtos = flatResult.ToList();

        var roots = departmentWithChildrenDtos
            .Where(dep => dep.ParentId == request.ParentId)
            .ToList();

        return roots;
    }
}