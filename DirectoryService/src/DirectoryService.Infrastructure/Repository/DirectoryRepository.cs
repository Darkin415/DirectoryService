using CSharpFunctionalExtensions;
using DirectoryService.Application.Interfaces;
using DirectoryService.Contacts.Errors;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.ValueObjects.LocationVO;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Infrastructure.Repository;

public class DirectoryRepository : IDirectoryRepository
{
    private readonly ApplicationDbContext _dbContext;

    public DirectoryRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
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
    public async Task<bool> AddressExistsAsync(Address address, CancellationToken cancellationToken)
    {
        return await _dbContext.Locations
            .SelectMany(l => l.Addresses)  
            .AnyAsync(a => a == address, cancellationToken);
    }
    public Task<bool> LocationNameExist(LocationName name, CancellationToken cancellationToken)
    {
        return _dbContext.Locations.AnyAsync(x => x.Name == name, cancellationToken);
    }
}