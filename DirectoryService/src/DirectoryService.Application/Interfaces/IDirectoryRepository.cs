using DirectoryService.Domain.Entities;

namespace DirectoryService.Application.Interfaces;

public interface IDirectoryRepository
{
    Task<Guid> AddLocation(Location location, CancellationToken cancellationToken);
}