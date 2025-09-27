using DirectoryService.Application.Department;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.IntergrationTests;

public class CreateDepartmentTests : DirectoryWebFactory
{
    [Fact]
    public async Task CreateDepartment_With_valid_data_should_succeed()
    {
        //arrange
        await using var scope = Services.CreateAsyncScope();
        
        var sut = scope.ServiceProvider.GetRequiredService<CreateDepartmentHandler>();
        var cancellationToken = CancellationToken.None;
        var command = new CreateDepartmentCommand("Подразделение", "dev", null, []);
        //act
        var result = await sut.Handle(command, cancellationToken);
        
        //assert

        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);
    }
}