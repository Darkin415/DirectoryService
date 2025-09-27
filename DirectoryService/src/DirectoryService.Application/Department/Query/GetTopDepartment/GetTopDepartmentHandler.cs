using Dapper;
using DirectoryService.Application.Interfaces;
using DirectoryService.Contracts.Dtos;

namespace DirectoryService.Application.Department.Query.GetTopDepartment;

public class GetTopDepartmentHandler
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public GetTopDepartmentHandler(ISqlConnectionFactory connectionFactory, ISqlConnectionFactory sqlConnectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<DepartmentTopDto>> Handle(CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.Create();

        const string sql = @"
        SELECT
        d.id,
        d.department_name AS ""DepartmentName"",
        COUNT(dp.position_id) AS ""PositionCount""
        FROM department.departments d
        JOIN department.department_position dp ON d.id = dp.department_id
        GROUP BY d.id, d.department_name
        ORDER BY ""PositionCount"" DESC
        LIMIT 5;
                 ";
        var topDepartments = await connection.QueryAsync<DepartmentTopDto>(sql, cancellationToken);

        return topDepartments.ToList();
    }
}