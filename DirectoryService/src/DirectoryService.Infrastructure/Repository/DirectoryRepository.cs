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

    public async Task<Guid> AddLocation(Location location, CancellationToken cancellationToken)
    {
        await _dbContext.Locations.AddAsync(location, cancellationToken);
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return location.Id.Value;
    }
}