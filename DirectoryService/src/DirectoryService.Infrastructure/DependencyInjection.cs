using System.Data;
using DirectoryService.Application.Database;
using DirectoryService.Application.Interfaces;
using DirectoryService.Infrastructure.Database;
using DirectoryService.Infrastructure.Options;
using DirectoryService.Infrastructure.Repository;
using DirectoryService.Infrastructure.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace DirectoryService.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);

        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        
        services.AddScoped<IPositionRepository, PositionRepository>();
        
        services.AddScoped<ILocationRepository, LocationRepository>();
        
        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
        
        services.AddScoped<ITransactionManager, TransactionManager>();

        // Подключение для Dapper
        services.AddScoped<IDbConnection>(sp =>
        {
            var connectionString = configuration
                .GetSection(PostgreSQL.SECTION)
                .GetValue<string>(PostgreSQL.CONNECTION_STRING);

            return new NpgsqlConnection(connectionString);
        });
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration
            .GetSection(PostgreSQL.SECTION)
            .GetValue<string>(PostgreSQL.CONNECTION_STRING);

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
    }
}

