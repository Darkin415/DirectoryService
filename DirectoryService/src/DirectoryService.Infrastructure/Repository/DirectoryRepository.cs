using CSharpFunctionalExtensions;
using DirectoryService.Application.Interfaces;
using DirectoryService.Domain.Entities;

namespace DirectoryService.Infrastructure.Repository;

public class DirectoryRepository : IDirectoryRepository
{
    private readonly ApplicationDbContext _dbContext;

    public DirectoryRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UnitResult<string>> AddLocation(Location location, CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.Locations.AddAsync(location, cancellationToken);
        
            await _dbContext.SaveChangesAsync(cancellationToken);

            return UnitResult.Success<string>();
        }
        
        catch (Exception ex)
        {
            return UnitResult.Failure<string>($"An error occurred while adding the location:{ex.Message}");
        }
    }
}