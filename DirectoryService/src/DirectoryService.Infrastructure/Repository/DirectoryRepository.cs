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

    public async Task<Result<Guid, ErrorList>> AddLocation(Location location, CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.Locations.AddAsync(location, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return location.Id.Value;
        }
        catch (Exception ex)
        {
            return Errors.General.ValueIsInvalid().ToErrorList();
        }
    }
    
    public async Task<bool> AllLocationsExistAsync(List<Guid> locationIds, CancellationToken cancellationToken)
    {
        var idsList = locationIds.ToList();
        var count = await _dbContext.Locations
            .CountAsync(x => idsList.Contains(x.Id.Value), cancellationToken);

        return count == idsList.Count;
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