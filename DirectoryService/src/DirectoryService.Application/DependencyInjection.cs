using DirectoryService.Application.Department;
using DirectoryService.Application.Department.Query;
using DirectoryService.Application.Department.Query.GetChildDepartments;
using DirectoryService.Application.Department.Query.GetRootDepartment;
using DirectoryService.Application.Department.Query.GetTopDepartment;
using DirectoryService.Application.Location.AddLocation;
using DirectoryService.Application.Location.GetLocationWithPagination;
using DirectoryService.Application.Location.ReplacementLocation;
using DirectoryService.Application.Location.UpdateLocation;
using DirectoryService.Application.Position;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<AddLocationsHandler>();
        services.AddScoped<CreateDepartmentHandler>();
        services.AddScoped<AddPositionHandler>();
        services.AddScoped<UpdateLocationHandler>();
        services.AddScoped<ReplacementDepartmentHandler>();
        services.AddScoped<GetLocationWIthPaginationHandler>();
        services.AddScoped<GetTopDepartmentHandler>();
        services.AddScoped<GetRootDepartmentHandler>();
        services.AddScoped<GetChildDepartmentHandler>();
    }
}