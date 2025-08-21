using CSharpFunctionalExtensions;
using DirectoryService.Domain.Entities;

namespace DirectoryService.Application.Interfaces;

public interface IDirectoryRepository
{
    Task<UnitResult<string>> AddLocation(Location location, CancellationToken cancellationToken);
}