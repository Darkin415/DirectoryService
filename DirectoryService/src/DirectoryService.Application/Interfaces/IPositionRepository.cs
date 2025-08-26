using CSharpFunctionalExtensions;
using DirectoryService.Contacts.Errors;
using DirectoryService.Domain.ValueObjects.PositionVO;

namespace DirectoryService.Application.Interfaces;

public interface IPositionRepository
{
    Task<Result<Guid, ErrorList>> AddPositionAsync(Domain.Entities.Position position, CancellationToken cancellationToken);
    
    Task<bool> ExistsActiveByNameAsync(PositionName name, CancellationToken cancellationToken);
}