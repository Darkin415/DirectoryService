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
            return validationResult.Errors.ToErrors();
                
        var name = new LocationName(command.Name);
        
        var nameResult = await _repository.LocationNameExist(name, cancellationToken);
        if(nameResult)
            return Errors.General.AlreadyExist("Location").ToErrorList();
        
        var timeZone = new TimeZone(command.TimeZone);
        
        var address = new Address(
            command.Address.Country, 
            command.Address.City, 
            command.Address.Street, 
            command.Address.Building, 
            command.Address.RoomNumber);

       var addressResult = await _repository.AddressExistsAsync(address, cancellationToken);
       if (addressResult)
           return Errors.General.AlreadyExist("Address").ToErrorList();
       
        var location = new Domain.Entities.Location(name, timeZone, address);
        
        var locationResult = await _repository.AddLocation(location, cancellationToken);
        if (locationResult.IsFailure)
            return locationResult.Error;
        
        _logger.LogInformation($"Location {name} created");

        return location.Id.Value;

    }
}