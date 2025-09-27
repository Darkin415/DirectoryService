using System.Data;
using CSharpFunctionalExtensions;
using DirectoryService.Application.Database;
using DirectoryService.Contracts.Errors;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure.Database;

public class TransactionScope : ITransactionScope, IDisposable
{
    private readonly IDbTransaction _dbTransaction;
    private readonly ILogger<TransactionScope> _logger;
    public TransactionScope(IDbTransaction dbTransaction, ILogger<TransactionScope> logger)
    {
        _dbTransaction = dbTransaction;
        _logger = logger;
    }

    public UnitResult<Error> Commit(CancellationToken cancellationToken)
    {
        try
        {
            _dbTransaction.Commit();
            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while committing transaction");
            return Error.Failure("commit.error", "Commit error");
        }
    }

    public UnitResult<Error> Rollback(CancellationToken cancellationToken)
    {
        try
        {
            _dbTransaction.Rollback();
            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while rolling back transaction");
            return Error.Conflict("rollback.error", "Rollback error");
        }
    }

    public void Dispose()
    {
        _dbTransaction.Dispose();
    }
}