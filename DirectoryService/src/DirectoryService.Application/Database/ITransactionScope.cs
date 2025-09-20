using CSharpFunctionalExtensions;
using DirectoryService.Contracts.Errors;

namespace DirectoryService.Application.Database;

public interface ITransactionScope : IDisposable
{
    UnitResult<Error> Commit(CancellationToken cancellationToken);
    UnitResult<Error> Rollback(CancellationToken cancellationToken);
}