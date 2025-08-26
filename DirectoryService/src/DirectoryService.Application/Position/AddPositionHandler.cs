using CSharpFunctionalExtensions;
using DirectoryService.Application.Interfaces;
using DirectoryService.Contacts.Errors;
using DirectoryService.Contacts.Validation;
using DirectoryService.Domain.Entities;
using DirectoryService.Domain.ValueObjects.DepartmentVO;
using DirectoryService.Domain.ValueObjects.PositionVO;
using FluentValidation;

namespace DirectoryService.Application.Position;

public class AddPositionHandler
{
   private readonly IDirectoryRepository  _repository;
   private readonly IValidator<AddPositionCommand> _validator;
   private readonly IPositionRepository _positionRepository;
   
    public AddPositionHandler(
        IDirectoryRepository repository, 
        IValidator<AddPositionCommand> validator, 
        IPositionRepository positionRepository)
    {
        _repository = repository;
        _validator = validator;
        _positionRepository = positionRepository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(AddPositionCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var positionResult = await _positionRepository.ExistsActiveByNameAsync(PositionName.Create(command.Name).Value, cancellationToken);
        if (positionResult)
            return Errors.General.AlreadyExist("Position").ToErrorList();
        
        var departmentIdResults = command.DepartmentIds
            .Distinct()
            .Select(DepartmentId.Create) 
            .ToList();
        
        var departmentIds = departmentIdResults
            .Select(r => r.Value)
            .ToList();

        var departmentIdsResult = await _repository.GetDepartmentsById(departmentIds, cancellationToken);
        if (departmentIdsResult.IsFailure)
            return Errors.General.NotFound().ToErrorList();
        
        var positionName = PositionName.Create(command.Name);
        
        var description = string.IsNullOrWhiteSpace(command.Description) 
            ? null 
            : Description.Create(command.Description).Value;
        
        var distinctDepartmentIds = command.DepartmentIds.Distinct().ToList();
        
        var position = new Domain.Entities.Position(positionName.Value, description);
        
        var departmentPositions = command.DepartmentIds
            .Select(departmentId => new DepartmentPosition(position.Id, DepartmentId.Create(departmentId).Value))
            .ToList();;
        
        var addPositionsResult = position.AddDepartmentPositions(departmentPositions);
        if (addPositionsResult.IsFailure)
            return addPositionsResult.Error.ToErrorList();
        
        var postionResult = await _positionRepository.AddPositionAsync(position, cancellationToken);

        return position.Id.Value;
    }
}