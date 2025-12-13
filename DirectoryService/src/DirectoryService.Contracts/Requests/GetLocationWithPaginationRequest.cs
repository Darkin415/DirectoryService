namespace DirectoryService.Contracts.Requests;

public record GetLocationWithPaginationRequest(
    string? Search,
    string? Country,
    string? City,
    bool? IsActive,
    List<Guid>? LocationIds,
    int Page = 1,
    int PageSize = 20);

public record PaginationRequest(
    int Page = 1,
    int PageSize = 20);