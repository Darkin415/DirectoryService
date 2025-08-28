using System.Data;
using CSharpFunctionalExtensions;
using DirectoryService.Application.Database;
using DirectoryService.Contacts.Errors;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure.Database;

public class TransactionManager : ITransactionManager
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TransactionManager> _logger;
    private readonly ILoggerFactory _loggerFactory;

    public TransactionManager(
        ApplicationDbContext context,
        ILogger<TransactionManager> logger,
        ILoggerFactory loggerFactory)
    {
        _context = context;
        _logger = logger;
        _loggerFactory = loggerFactory;
    }

    public async Task<Result<ITransactionScope, Error>>
        BeginTransactionAsync(CancellationToken cancellationToken)
    {
        try
        {
            var transaction = await _context.Database
                .BeginTransactionAsync(cancellationToken);

            var transactionScope = new TransactionScope(
                transaction.GetDbTransaction(),
                _loggerFactory.CreateLogger<TransactionScope>());

            return transactionScope;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to begin transaction");
            return Error.Failure("database", "Faled to begin transaction");
        }
    }

    public async Task<UnitResult<Error>> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save changes to the database");
            return Error.Failure("database", "Failed to save changes: " + ex.Message);
        }
    }
}
