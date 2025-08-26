using DirectoryService.Application.Department;
using DirectoryService.Application.Location.AddLocation;
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
    }
}