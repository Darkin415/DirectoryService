using CSharpFunctionalExtensions;
using DirectoryService.Contracts.Errors;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.ValueObjects.DepartmentVO;
using DirectoryService.Domain.ValueObjects.LocationVO;
using Path = DirectoryService.Domain.ValueObjects.DepartmentVO.Path;

namespace DirectoryService.Application.Interfaces;

public interface IDepartmentRepository
{
    Task<Result<List<Domain.Entities.Department>, Error>> GetDepartmentsById(
        List<DepartmentId> departmentIds,
        CancellationToken cancellationToken);

    Task<UnitResult<Error>> MoveDepartments(
        Domain.Entities.Department department,
        Path oldPath);

    Task<UnitResult<Error>> LockDescendants(
        Path path);
    
    Task<Result<Guid, ErrorList>> AddDepartment(Domain.Entities.Department department, CancellationToken cancellationToken);

    Task<Result<Domain.Entities.Department, Error>> GetByIdWithLockAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> IsIdentifierExistAsync(string identifier, CancellationToken cancellationToken);
    
    Task<bool> LocationNameExist(LocationName name, CancellationToken cancellationToken);
    
    Task<bool> AddressExistsAsync(Address address, CancellationToken cancellationToken);

    Task<UnitResult<Error>> SaveChangesAsync(CancellationToken cancellationToken);

    Task<UnitResult<ErrorList>> IsDescendant(
        DepartmentId rootDepartmentId,
        DepartmentId candidateChildDepartmentId);
    
    Task<Result<Domain.Entities.Department, Error>> GetDepartmentById(DepartmentId departmentId, CancellationToken cancellationToken);
}