namespace DirectoryService.Application.Database;

public interface IReadApplicationDbContext
{ 
    IQueryable<Domain.Entities.Location> ReadLocations { get; }
}