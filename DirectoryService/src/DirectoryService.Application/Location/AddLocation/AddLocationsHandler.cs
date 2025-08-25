using CSharpFunctionalExtensions;
using DirectoryService.Application.Interfaces;
using DirectoryService.Contacts.Errors;
using DirectoryService.Contacts.Validation;
using DirectoryService.Domain.ValueObjects.LocationVO;
using FluentValidation;
using Microsoft.Extensions.Logging;
using TimeZone = DirectoryService.Domain.ValueObjects.LocationVO.TimeZone;

namespace DirectoryService.Application.Location.AddLocation;

public class AddLocationsHandler
{
    private readonly IDirectoryRepository _repository;
    private readonly ILogger<AddLocationsHandler> _logger;
    private readonly IValidator<AddLocationCommand> _validator;

    public AddLocationsHandler(
        IDirectoryRepository repository, 
        ILogger<AddLocationsHandler> logger, IValidator<AddLocationCommand> validator)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
    }
    public async Task<Result<Guid, ErrorList>> Handle(AddLocationCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();
                
        var name = LocationName.Create(command.Name);
        if (name.IsFailure)
            return name.Error.ToErrorList();
        
        var nameResult = await _repository.LocationNameExist(name.Value, cancellationToken);
        if(nameResult)
            return Errors.General.AlreadyExist("Location").ToErrorList();
        
        var timeZone =  TimeZone.Create(command.TimeZone);
        if(timeZone.IsFailure)
            return timeZone.Error.ToErrorList();
        
        var address =  Address.Create(
            command.Address.Country, 
            command.Address.City, 
            command.Address.Street, 
            command.Address.Building, 
            command.Address.RoomNumber);
        if(address.IsFailure)
            return address.Error.ToErrorList();

        var addressExist = await _repository.AddressExistsAsync(address.Value, cancellationToken);
        if(addressExist)
            return Errors.General.AlreadyExist("Address").ToErrorList();
        
        var location = new Domain.Entities.Location(name.Value, timeZone.Value, address.Value);
        
        
        var locationResult = await _repository.AddLocation(location, cancellationToken);
        if (locationResult.IsFailure)
            return locationResult.Error;
        
        _logger.LogInformation($"Location {name} created");

        return location.Id.Value;

    }
}