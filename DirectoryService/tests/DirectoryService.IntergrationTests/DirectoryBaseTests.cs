using DirectoryService.Application.Department;
using DirectoryService.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.IntergrationTests;

public class DirectoryBaseTests : IClassFixture<DirectoryWebFactory>, IAsyncLifetime
{
    private readonly Func<Task> _resetDatabase;
    protected IServiceProvider Services { get; set; }
    
    protected DirectoryBaseTests(DirectoryWebFactory factory)
    {
        Services = factory.Services;
        _resetDatabase = factory.ResetDatabaseAsync;
    }
    
    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await _resetDatabase();
    }
    
    protected async Task<T> ExecuteInDb<T>(Func<ApplicationDbContext, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();
        
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await action(dbContext);
    }
    
    protected async Task ExecuteDb(Func<ApplicationDbContext, Task> action)
    {
        await using var scope = Services.CreateAsyncScope();
        
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await action(dbContext);
    }
    
    public async Task<T> ExecuteHandler<T>(Func<CreateDepartmentHandler, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();
        
        var sut = scope.ServiceProvider.GetRequiredService<CreateDepartmentHandler>();

        return await action(sut);
    }
}