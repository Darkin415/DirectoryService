using CSharpFunctionalExtensions;
using DirectoryService.Contacts.Errors;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.ValueObjects.DepartmentVO;
using DirectoryService.Domain.ValueObjects.LocationVO;

namespace DirectoryService.Application.Interfaces;

public interface IDirectoryRepository
{
    Task<Result<Guid, ErrorList>> AddPositionAsync(Domain.Entities.Position position, CancellationToken cancellationToken);
    Task<Result<Guid, ErrorList>> AddLocation(Domain.Entities.Location location, CancellationToken cancellationToken);
    
    Task<Result<Guid, ErrorList>> AddDepartment(Domain.Entities.Department department, CancellationToken cancellationToken);
    
    Task<bool> LocationNameExist(LocationName name, CancellationToken cancellationToken);
    
    // Task<bool> IdentifierExistsAsync(Guid? parentId, Identifier identifier,
    //     CancellationToken cancellationToken);

    // Task<bool> AllLocationExistsAsync(List<Guid> locations, CancellationToken cancellationToken);

    Task<bool> AddressExistsAsync(Address address, CancellationToken cancellationToken);
    
    Task<Result<Domain.Entities.Department, Error>> GetDepartmentById(DepartmentId departmentId, CancellationToken cancellationToken);
}