using CSharpFunctionalExtensions;
using DirectoryService.Application.Interfaces;
using DirectoryService.Contacts.Errors;
using DirectoryService.Contacts.Validation;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.ValueObjects.DepartmentVO;
using DirectoryService.Domain.ValueObjects.LocationVO;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.Location.UpdateLocation;

public class UpdateLocationHandler
{
    private readonly IValidator<UpdateLocationCommand> _validator;
    private readonly ILogger<UpdateLocationHandler> _logger;
    private readonly ILocationRepository _locationRepository;
    private readonly IDirectoryRepository _directoryRepository;
    
    public UpdateLocationHandler(
        IValidator<UpdateLocationCommand> validator, 
        ILogger<UpdateLocationHandler> logger, 
        ILocationRepository locationRepository, IDirectoryRepository directoryRepository)
    {
        _validator = validator;
        _logger = logger;
        _locationRepository = locationRepository;
        _directoryRepository = directoryRepository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(UpdateLocationCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if(validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var departmentId = DepartmentId.Create(command.DepartmentId);
        
        var departmentLocationsResult = MapToDepartmentLocations(departmentId.Value, command.LocationIds).Value;
        
        var department = await _directoryRepository.GetDepartmentById(departmentId.Value, cancellationToken);

        var result = department.Value.UpdateDepartmentLocations(departmentLocationsResult);

        await _directoryRepository.SaveChangesAsync(cancellationToken);

        return department.Value.Id.Value;

    }

    public Result<List<DepartmentLocation>, Error> MapToDepartmentLocations(DepartmentId departmentId,
        List<Guid> locationIds)
    {
        var departmentLocationsResult = locationIds
            .Distinct()
            .Select(locationId => DepartmentLocation.Create(new LocationId(locationId), departmentId))
            .ToList();
        
        var departmentLocations = departmentLocationsResult.Select(r => r.Value).ToList();
        
        return departmentLocations;
    }
    
}