using System.Data;
using System.Text.Json;
using CSharpFunctionalExtensions;
using Dapper;
using DirectoryService.Application.Interfaces;
using DirectoryService.Contacts.Errors;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.ValueObjects.DepartmentVO;
using DirectoryService.Domain.ValueObjects.LocationVO;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Infrastructure.Repository;

public class DirectoryRepository : IDirectoryRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IDbConnection _dbConnection;

    public DirectoryRepository(ApplicationDbContext dbContext, IDbConnection dbConnection)
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
            return Errors.General.LocationNotFound("");

        return departments;
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
        return  await _dbContext.Departments.AnyAsync(d => d.Identifier.Value == identifier);
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

    public async Task<Result<Department, Error>> GetDepartmentById(DepartmentId departmentId,
        CancellationToken cancellationToken)
    {
        var department = await _dbContext.Departments
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