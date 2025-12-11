using System.Data;
using CSharpFunctionalExtensions;
using DirectoryService.Application.Database;
using DirectoryService.Contracts.Errors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure.Database;

public class TransactionManager : ITransactionManager
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<ITransactionManager> _logger;


    public TransactionManager(
        ApplicationDbContext dbContext,
        ILoggerFactory loggerFactory,
        ILogger<ITransactionManager> logger)
    {
        _dbContext = dbContext;
        _loggerFactory = loggerFactory;
        _logger = logger;
    }

    public async Task<Result<ITransactionScope, Error>> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        try
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            var loggerForTransaction = _loggerFactory.CreateLogger<TransactionScope>();
            var transactionScope = new TransactionScope(transaction.GetDbTransaction(), loggerForTransaction);
            return transactionScope;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Error.Conflict("error.transaction", "An error occured while trying to create a new transaction");
        }
    }

    public async Task<UnitResult<Error>> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while saving changes");
            return Error.Failure("error.transaction", "An error occured while saving changes");
        }
    }
}