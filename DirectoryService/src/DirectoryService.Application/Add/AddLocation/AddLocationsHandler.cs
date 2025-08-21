using CSharpFunctionalExtensions;
using DirectoryService.Application.Interfaces;
using DirectoryService.Contacts.Errors;
using DirectoryService.Domain;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.ValueObjects.LocationVO;
using Microsoft.Extensions.Logging;
using TimeZone = DirectoryService.Domain.ValueObjects.LocationVO.TimeZone;

namespace DirectoryService.Application.Add.AddLocation;

public class AddLocationsHandler
{
    private readonly IDirectoryRepository _repository;
    private readonly ILogger<AddLocationsHandler> _logger;

    public AddLocationsHandler(IDirectoryRepository repository, ILogger<AddLocationsHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    public async Task<Result<Guid, Error>> Handle(AddLocationCommand command, CancellationToken cancellationToken)
    {
        var name = LocationName.Create(command.Name);
        if(name.IsFailure)
            return Errors.General.ValueIsInvalid("Name");
        
        var timeZone = TimeZone.Create(command.TimeZone);
        if(timeZone.IsFailure)
            return Errors.General.ValueIsInvalid("TimeZone");
        
        var address = Address.Create(
            command.Address.Country, 
            command.Address.City, 
            command.Address.Street, 
            command.Address.Building, 
            command.Address.RoomNumber);
        
        var location = new Location(name.Value, timeZone.Value, address.Value);
        
        var locationResult = await _repository.AddLocation(location, cancellationToken);
        
        _logger.LogInformation($"Location {name.Value} created");

        return location.Id.Value;

    }
}