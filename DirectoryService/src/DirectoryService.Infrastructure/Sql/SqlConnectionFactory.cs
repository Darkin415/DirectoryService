using System.Data;
using DirectoryService.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace DirectoryService.Infrastructure.Sql;

public class SqlConnectionFactory(IConfiguration configuration) : ISqlConnectionFactory
{
    public IDbConnection Create() =>
        new NpgsqlConnection(configuration.GetConnectionString("Database"));
}