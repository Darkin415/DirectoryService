using System.Data;
using CSharpFunctionalExtensions;
using DirectoryService.Application.Interfaces;
using DirectoryService.Contacts.Errors;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.ValueObjects.PositionVO;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Infrastructure.Repository;

public class PositionRepository : IPositionRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IDbConnection _dbConnection;

    public PositionRepository(ApplicationDbContext dbContext, IDbConnection dbConnection)
    {
        _dbContext = dbContext;
        _dbConnection = dbConnection;
    }
    
    public async Task<Result<Guid, ErrorList>> AddPositionAsync(Position position, CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.Positions.AddAsync(position, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return position.Id.Value;
        }
        catch (Exception ex)
        {
            
            return Errors.General.ValueIsInvalid().ToErrorList();
        }
    }

    public async Task<bool> ExistsActiveByNameAsync(PositionName name, CancellationToken cancellationToken)
    {
        return await _dbContext.Positions
            .AnyAsync(x => x.Name.Value == name.Value && x.IsActive, cancellationToken);
    }
}