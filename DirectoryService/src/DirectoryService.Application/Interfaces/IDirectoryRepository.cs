using CSharpFunctionalExtensions;
using DirectoryService.Contacts.Errors;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.ValueObjects.LocationVO;

namespace DirectoryService.Application.Interfaces;

public interface IDirectoryRepository
{
    Task<UnitResult<string>> AddLocation(Domain.Entities.Location location, CancellationToken cancellationToken);
    
    Task<bool> LocationNameExist(LocationName name, CancellationToken cancellationToken);

    Task<bool> AddressExistsAsync(Address address, CancellationToken cancellationToken);
}