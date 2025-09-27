using DirectoryService.Application.Location.AddLocation;
using DirectoryService.Contracts.Dtos;
using DirectoryService.Domain.ValueObjects.LocationVO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.IntergrationTests;

public class CreateLocationTests : DirectoryBaseTests
{
    protected CreateLocationTests(DirectoryWebFactory factory) : base(factory)
    {
    }

    public class AddLocationTests : DirectoryBaseTests
    {
        public AddLocationTests(DirectoryWebFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task AddLocation_WithValidData_ShouldSucceed()
        {
            // arrange
            var cancellationToken = CancellationToken.None;

            // act
            var result = await ExecuteLocationHandler(async sut =>
            {
                var command = new AddLocationCommand(
                    "Новая локация",
                    new AddressDto
                    {
                        Country = "Россия",
                        City = "Москва",
                        Street = "Ленина",
                        Building = "10",
                        RoomNumber = 15
                    },
                    "Europe/Moscow"
                );

                return await sut.Handle(command, cancellationToken);
            });

            // assert
            await ExecuteDb(async dbContext =>
            {
                var location = await dbContext.Locations.FirstOrDefaultAsync(
                    l => l.Id == new LocationId(result.Value), cancellationToken);

                Assert.NotNull(location);
                Assert.Equal(location.Id.Value, result.Value);
                Assert.True(result.IsSuccess);
                Assert.NotEqual(Guid.Empty, result.Value);
            });
        }
        
        [Fact]
        public async Task AddLocation_WithInvalidName_ShouldFail()
        {
            var cancellationToken = CancellationToken.None;

            var result = await ExecuteLocationHandler(async sut =>
            {
                var command = new AddLocationCommand(
                    "", // пустое имя
                    new AddressDto
                    {
                        Country = "Россия",
                        City = "Москва",
                        Street = "Ленина",
                        Building = "10",
                        RoomNumber = 15
                    },
                    "Europe/Moscow"
                );

                return await sut.Handle(command, cancellationToken);
            });

            Assert.True(result.IsFailure);
            Assert.Contains("value_is_invalid", result.Error.Select(e => e.Message));
        }

        [Fact]
        public async Task AddLocation_WithInvalidTimeZone_ShouldFail()
        {
            var cancellationToken = CancellationToken.None;

            var result = await ExecuteLocationHandler(async sut =>
            {
                var command = new AddLocationCommand(
                    "Локация",
                    new AddressDto
                    {
                        Country = "Россия",
                        City = "Москва",
                        Street = "Ленина",
                        Building = "10",
                        RoomNumber = 15
                    },
                    "не поддерживается" // неверная зона
                );

                return await sut.Handle(command, cancellationToken);
            });

            Assert.True(result.IsFailure);
            Assert.Contains("TimeZone", string.Join(',', result.Error.Select(e => e.Message)));
        }

        private async Task<T> ExecuteLocationHandler<T>(Func<AddLocationsHandler, Task<T>> action)
        {
            await using var scope = Services.CreateAsyncScope();
            var sut = scope.ServiceProvider.GetRequiredService<AddLocationsHandler>();
            return await action(sut);
        }
    }
}