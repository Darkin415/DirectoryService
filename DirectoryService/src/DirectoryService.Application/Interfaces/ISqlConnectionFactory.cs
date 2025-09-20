using System.Data;

namespace DirectoryService.Application.Interfaces;

public interface ISqlConnectionFactory
{
    IDbConnection Create();
}