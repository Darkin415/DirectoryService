using System.Data;
using CSharpFunctionalExtensions;
using DirectoryService.Application.Interfaces;
using DirectoryService.Contracts.Errors;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.ValueObjects.LocationVO;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Infrastructure.Repository;

public class LocationRepository : ILocationRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IDbConnection _dbConnection;

    public LocationRepository(ApplicationDbContext dbContext, IDbConnection dbConnection)
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
    
    public async Task<Result<List<Location>, Error>> GetLocationsById(
        List<LocationId> locationsIds,
        CancellationToken cancellationToken)
    {
        var distinctLocations = locationsIds.Distinct().ToList();

        var locations = await _dbContext.Locations
            .Where(l => distinctLocations.Contains(l.Id))
            .ToListAsync(cancellationToken);

        if (distinctLocations.Count != locations.Count)
            return Errors.General.LocationNotFound("");

        return locations;
    }
}