using CSharpFunctionalExtensions;
using DirectoryService.Contacts.Errors;
using DirectoryService.Domain.ValueObjects.LocationVO;

namespace DirectoryService.Application.Interfaces;

public interface ILocationRepository
{
    Task<Result<List<Domain.Entities.Location>, Error>> GetLocationsById(
        List<LocationId> locationsIds,
        CancellationToken cancellationToken);

    Task<Result<Guid, ErrorList>> AddLocation(Domain.Entities.Location location, CancellationToken cancellationToken);
}