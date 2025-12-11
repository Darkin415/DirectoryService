using CSharpFunctionalExtensions;
using DirectoryService.Application.Database;
using DirectoryService.Application.Interfaces;
using DirectoryService.Contracts.Errors;
using DirectoryService.Contracts.Validation;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.ValueObjects.DepartmentVO;
using DirectoryService.Domain.ValueObjects.LocationVO;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.Location.UpdateLocation;

public class UpdateLocationHandler
{
    private readonly ITransactionManager _transactionManager;
    private readonly IValidator<UpdateLocationCommand> _validator;
    private readonly ILogger<UpdateLocationHandler> _logger;
    private readonly ILocationRepository _locationRepository;
    private readonly IDepartmentRepository _departmentRepository;
    
    public UpdateLocationHandler(
        IValidator<UpdateLocationCommand> validator, 
        ILogger<UpdateLocationHandler> logger, 
        ILocationRepository locationRepository, 
        IDepartmentRepository departmentRepository, 
        ITransactionManager transactionManager)
    {
        _validator = validator;
        _logger = logger;
        _locationRepository = locationRepository;
        _departmentRepository = departmentRepository;
        _transactionManager = transactionManager;
    }

    public async Task<Result<Guid, ErrorList>> Handle(UpdateLocationCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if(validationResult.IsValid == false)
            return validationResult.ToErrorList();
        
        var departmentId = DepartmentId.Create(command.DepartmentId);
        
        var department = await _departmentRepository.GetDepartmentById(departmentId.Value, cancellationToken);
        if(department.IsFailure)
            return department.Error.ToErrorList();
        
        //создаю локации для проверки
        var locationIdResults = command.LocationIds
            .Distinct()
            .Select(LocationId.Create) 
            .ToList();
        
        var locationIds = locationIdResults
            .Select(r => r.Value)
            .ToList();
        // проверка локаций на существование
        var locationsResult = await _locationRepository
            .GetLocationsById(locationIds, cancellationToken);
        if(locationsResult.IsFailure)
            return locationsResult.Error.ToErrorList();
        
        // создаю departmentLocations
        var departmentLocations = command.LocationIds
            .Select(locationId => new DepartmentLocation(LocationId.Create(locationId).Value, department.Value.Id))
            .ToList();
        
        var transactionResult = await _transactionManager
            .BeginTransactionAsync(cancellationToken);
        if (transactionResult.IsFailure)
        {
            _logger.LogError(transactionResult.Error.Message);
            return transactionResult.Error.ToErrorList();
        }
        
        var transaction = transactionResult.Value;
        
        var result = department.Value.UpdateDepartmentLocations(departmentLocations);
        
        var saveChangesResult = await _transactionManager
            .SaveChangesAsync(cancellationToken);
        
        var commitResult = transaction.Commit(cancellationToken);
        if (commitResult.IsFailure)
        {
            _logger.LogError(commitResult.Error.Message);
            return commitResult.Error.ToErrorList();
        }
        
        return department.Value.Id.Value;
    }
}