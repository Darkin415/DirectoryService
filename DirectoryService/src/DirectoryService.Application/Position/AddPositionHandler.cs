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
   
    public AddPositionHandler(IDirectoryRepository repository, IValidator<AddPositionCommand> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> AddPosition(AddPositionCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command);
        if (!validationResult.IsValid)
            return validationResult.ToErrorList();
        
        var positionName = PositionName.Create(command.Name);
        
        var description = string.IsNullOrWhiteSpace(command.Description) 
            ? null 
            : Description.Create(command.Description).Value;
        
        var departmentIds = command.DepartmentIds;
        
        var position = new Domain.Entities.Position(positionName.Value, description);

        // протестить потом и попытаться сделать проверку одинаковой локации и одинакого identifire 
        var departmentPositions = command.DepartmentIds
            .Select(departmentId => new DepartmentPosition(position.Id, DepartmentId.Create(departmentId).Value))
            .ToList();;
        
        var addPositionsResult = position.AddDepartmentPositions(departmentPositions);
        if (addPositionsResult.IsFailure)
            return addPositionsResult.Error.ToErrorList();

        return position.Id.Value;
    }
}