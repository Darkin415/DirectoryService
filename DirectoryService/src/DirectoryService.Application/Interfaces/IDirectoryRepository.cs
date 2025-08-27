using CSharpFunctionalExtensions;
using DirectoryService.Contacts.Errors;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.ValueObjects.DepartmentVO;
using DirectoryService.Domain.ValueObjects.LocationVO;

namespace DirectoryService.Application.Interfaces;

public interface IDirectoryRepository
{
    Task<Result<List<Domain.Entities.Department>, Error>> GetDepartmentsById(
        List<DepartmentId> departmentIds,
        CancellationToken cancellationToken);
    
    Task<Result<Guid, ErrorList>> AddDepartment(Domain.Entities.Department department, CancellationToken cancellationToken);

    Task<bool> IsIdentifierExistAsync(string identifier, CancellationToken cancellationToken);
    
    Task<bool> LocationNameExist(LocationName name, CancellationToken cancellationToken);
    
    Task<bool> AddressExistsAsync(Address address, CancellationToken cancellationToken);

    Task<UnitResult<Error>> SaveChangesAsync(CancellationToken cancellationToken);
    
    Task<Result<Domain.Entities.Department, Error>> GetDepartmentById(DepartmentId departmentId, CancellationToken cancellationToken);
}