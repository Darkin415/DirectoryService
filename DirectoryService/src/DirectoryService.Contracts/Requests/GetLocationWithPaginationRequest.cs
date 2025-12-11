namespace DirectoryService.Contracts.Requests;

public record GetLocationWithPaginationRequest(
    string? Search,
    string? Country,
    string? City,
    bool? IsActive,
    List<Guid>? LocationIds,
    PaginationRequest Pagination);

public record PaginationRequest(
    int Page = 1,
    int PageSize = 20);