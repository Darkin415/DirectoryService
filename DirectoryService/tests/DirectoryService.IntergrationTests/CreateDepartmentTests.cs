using DirectoryService.Application.Department;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.ValueObjects.DepartmentVO;
using DirectoryService.Domain.ValueObjects.LocationVO;
using DirectoryService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TimeZone = DirectoryService.Domain.ValueObjects.LocationVO.TimeZone;

namespace DirectoryService.IntergrationTests;

public class CreateDepartmentTests : DirectoryBaseTests
{
    public CreateDepartmentTests(DirectoryWebFactory factory) : base(factory)
    {}
    
    [Fact]
    public async Task CreateDepartment_With_valid_data_should_succeed()
    {
        //arrange
        var locationId = await CreateLocation();
        
        var cancellationToken = CancellationToken.None;
        
        //act
        var result = await ExecuteHandler((sut) =>
        {
            var command = new CreateDepartmentCommand(
                "Подразделение", "dezxc", null, [locationId.Value]);
            
            return sut.Handle(command, cancellationToken);
        });
        
        //assert
        await ExecuteDb(async dbContext =>
        {
            var department =
                await dbContext.Departments.FirstAsync(d => d.Id == new DepartmentId(result.Value), cancellationToken);
            Assert.NotNull(department);
            Assert.Equal(department.Id.Value, result.Value);
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Value);
        });
    }

    private async Task<LocationId> CreateLocation()
    {
        return await ExecuteInDb(async dbContext =>
        {
            var location = new Location(LocationName.Create("локация").Value, TimeZone.Create("Europe/Moscow").Value,
                new Address("москва", "asdf", "asdf", "asdf", 12));

            dbContext.Locations.Add(location);
            await dbContext.SaveChangesAsync();

            return location.Id;
        });
    }

    private async Task<T> ExecuteHandler<T>(Func<CreateDepartmentHandler, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();
        
        var sut = scope.ServiceProvider.GetRequiredService<CreateDepartmentHandler>();

        return await action(sut);
    }
    
   
    
}