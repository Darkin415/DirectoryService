using System.Data;
using System.Text.Json;
using CSharpFunctionalExtensions;
using Dapper;
using DirectoryService.Application.Interfaces;
using DirectoryService.Contracts.Errors;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.ValueObjects.DepartmentVO;
using DirectoryService.Domain.ValueObjects.LocationVO;
using Microsoft.EntityFrameworkCore;
using Path = DirectoryService.Domain.ValueObjects.DepartmentVO.Path;

namespace DirectoryService.Infrastructure.Repository;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IDbConnection _dbConnection;

    public DepartmentRepository(ApplicationDbContext dbContext, IDbConnection dbConnection)
    {
        _dbContext = dbContext;
        _dbConnection = dbConnection;
    }

    public async Task<Result<List<Department>, Error>> GetDepartmentsById(
        List<DepartmentId> departmentIds,
        CancellationToken cancellationToken)
    {
        var distinctDepartments = departmentIds.Distinct().ToList();

        var departments = await _dbContext.Departments
            .Where(l => distinctDepartments.Contains(l.Id))
            .ToListAsync(cancellationToken);

        if (distinctDepartments.Count != departments.Count)
            return Errors.General.NotFound();

        return departments;
    }

    public async Task<UnitResult<Error>> MoveDepartments(
        Department department,
        Path oldPath)
    {
        var connection = _dbContext.Database.GetDbConnection();

        const string dapperSql =
            """
            UPDATE department.departments
            SET depth = @departmentDepth + (depth - nlevel(@oldPath::ltree) + 1),
                path = @departmentPath::ltree || subpath(path, nlevel(@oldPath::ltree))
            WHERE path <@ @oldPath::ltree
            AND path != @oldPath::ltree
            """;

        await connection.ExecuteAsync(dapperSql, new
        {
            departmentDepth = department.Depth,
            departmentPath = department.Path.Value,
            oldPath = oldPath.Value
        });

        return UnitResult.Success<Error>();
    }

    public async Task<UnitResult<Error>> LockDescendants(
        Path path)
    {
        var connection = _dbContext.Database.GetDbConnection();

        const string dapperSql =
            """
            SELECT *
            FROM department.departments
            WHERE path <@ @path::ltree
            FOR UPDATE
            """;

        await connection.ExecuteAsync(dapperSql, new
        {
            path = path.Value,
        });

        return UnitResult.Success<Error>();
    }

    public async Task<Result<Department, Error>> GetByIdWithLockAsync(Guid id, CancellationToken cancellationToken)
    {
        var department = await _dbContext.Departments
            .FromSqlInterpolated($"SELECT * FROM department.departments WHERE id = {id} FOR UPDATE")
            .FirstOrDefaultAsync(cancellationToken);

        if (department == null)
            return Errors.General.NotFound(id);

        return department;
    }

    public async Task<Result<Guid, ErrorList>> AddDepartment(Department department, CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.Departments.AddAsync(department, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return department.Id.Value;
        }

        catch (Exception ex)
        {
            return Errors.General.ValueIsInvalid().ToErrorList();
        }
    }

    public async Task<bool> IsIdentifierExistAsync(string identifier, CancellationToken cancellationToken)
    {
        return await _dbContext.Departments.AnyAsync(d => d.Identifier.Value == identifier);
    }

    public Task<bool> AddressExistsAsync(Address address, CancellationToken cancellationToken)
    {
        return _dbContext.Locations.AnyAsync(l =>
                l.Address.Country == address.Country &&
                l.Address.City == address.City &&
                l.Address.Street == address.Street &&
                l.Address.Building == address.Building &&
                l.Address.RoomNumber == address.RoomNumber,
            cancellationToken);
    }

    public async Task<UnitResult<Error>> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            return Errors.General.ValueIsInvalid();
        }
    }

    public async Task<UnitResult<ErrorList>> IsDescendant(
        DepartmentId rootDepartmentId,
        DepartmentId candidateChildDepartmentId)
    {
        var result = await _dbContext.Departments
            .AnyAsync(d => 
                d.Id == rootDepartmentId &&
                d.ChildrenDepartments.Any(cd => cd.Id == candidateChildDepartmentId));
        if(result)
            return Errors.General.ItsChild().ToErrorList();
        
        return Result.Success<ErrorList>();
    }


    public async Task<Result<Department, Error>> GetDepartmentById(DepartmentId departmentId,
        CancellationToken cancellationToken)
    {
        var department = await _dbContext.Departments
            .Include(x => x.DepartmentLocations)
            .FirstOrDefaultAsync(x => x.Id == departmentId, cancellationToken);

        if (department == null)
            return Errors.General.NotFound(departmentId.Value);

        return department;
    }

    public Task<bool> LocationNameExist(LocationName name, CancellationToken cancellationToken)
    {
        return _dbContext.Locations.AnyAsync(x => x.Name == name, cancellationToken);
    }
}