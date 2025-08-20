using DirectoryService.Application.Add.AddLocation;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<AddLocationsHandler>();
    }
}