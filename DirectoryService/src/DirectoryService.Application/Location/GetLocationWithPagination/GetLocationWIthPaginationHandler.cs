using DirectoryService.Application.Database;
using DirectoryService.Application.Interfaces;
using DirectoryService.Contracts.Requests;
using DirectoryService.Domain.ValueObjects.LocationVO;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Application.Location.GetLocationWithPagination;

public class GetLocationWIthPaginationHandler
{
    private readonly IReadApplicationDbContext _readDbContext;

    public GetLocationWIthPaginationHandler(IReadApplicationDbContext dbContext, IReadApplicationDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PaginationLocationResponse?> Handle(
        GetLocationWithPaginationRequest request,
        CancellationToken cancellationToken)
    {
        //запрос на получение локаций
        var locationsQuery = _readDbContext.ReadLocations.AsQueryable();
        
        locationsQuery = locationsQuery.OrderBy(e => e.CreatedAt);
       
        //фильтрация по имени
        if(!string.IsNullOrWhiteSpace(request.Search))
            locationsQuery = locationsQuery.Where(e => EF.Functions.Like(e.Name.Value.ToLower(), $"%{request.Search.ToLower()}%"));
        
        //фильтрация по стране
        if(!string.IsNullOrWhiteSpace(request.Country))
            locationsQuery = locationsQuery.Where(e => EF.Functions.Like(e.Address.Country.ToLower(), $"%{request.Country.ToLower()}%"));
        
        //фильтрация по городу
        if(!string.IsNullOrWhiteSpace(request.City))
            locationsQuery = locationsQuery.Where(e => EF.Functions.Like(e.Address.City.ToLower(), $"%{request.City.ToLower()}%"));
        
        if (request.LocationIds is { Count: > 0 })
        {
            locationsQuery = locationsQuery.Where(e => request.LocationIds.Contains(e.Id.Value));
        }
        
        if (request.IsActive.HasValue)
            locationsQuery = locationsQuery.Where(e => e.IsActive == request.IsActive.Value);
        
        var totalCount = await locationsQuery.LongCountAsync(cancellationToken);
        
        locationsQuery = locationsQuery
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize);
        
        var locations = await locationsQuery.OrderByDescending(l => l.CreatedAt).ToListAsync(cancellationToken);

        if (locations.Count == 0)
        {
            return null;
        }
        
        int totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
        
        var locationsDto = locations
            .Select(loc => new LocationDto
        {
            Name = loc.Name.Value,
            Country = loc.Address.Country,
            City = loc.Address.City,
            Street = loc.Address.Street,
            Building = loc.Address.Building,
            RoomNumber = loc.Address.RoomNumber,
            TimeZone = loc.TimeZone,         
            IsActive = loc.IsActive,
            CreatedAt = loc.CreatedAt,
            UpdatedAt = loc.UpdatedAt
        }).ToList();
        
        return new PaginationLocationResponse(locationsDto, totalCount, request.Page, request.PageSize, totalPages);
    }
}