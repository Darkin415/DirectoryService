using CSharpFunctionalExtensions;
using DirectoryService.Contacts.Errors;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.ValueObjects.DepartmentVO;
using DirectoryService.Domain.ValueObjects.LocationVO;

namespace DirectoryService.Application.Interfaces;

public interface IDirectoryRepository
{
    Task<Result<Guid, ErrorList>> AddLocation(Domain.Entities.Location location, CancellationToken cancellationToken);
    
    Task<bool> LocationNameExist(LocationName name, CancellationToken cancellationToken);
    
    Task<bool> AllLocationsExistAsync(List<Guid> locationIds, CancellationToken cancellationToken);

    Task<bool> AddressExistsAsync(Address address, CancellationToken cancellationToken);
    
    Task<Result<Domain.Entities.Department, Error>> GetDepartmentById(DepartmentId departmentId, CancellationToken cancellationToken);
}