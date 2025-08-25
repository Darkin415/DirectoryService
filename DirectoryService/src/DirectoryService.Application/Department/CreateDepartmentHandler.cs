// using CSharpFunctionalExtensions;
// using DirectoryService.Application.Interfaces;
// using DirectoryService.Application.Location.AddLocation;
// using DirectoryService.Contacts.Errors;
// using DirectoryService.Contacts.Validation;
// using DirectoryService.Domain.Entities;
// using DirectoryService.Domain.ValueObjects.DepartmentVO;
// using DirectoryService.Domain.ValueObjects.LocationVO;
// using FluentValidation;
// using Microsoft.Extensions.Logging;
//
// namespace DirectoryService.Application.Department;
//
// public class CreateDepartmentHandler
// {
//     private readonly IValidator<CreateDepartmentCommand> _validator;
//     private readonly ILogger<CreateDepartmentHandler> _logger;
//     private readonly IDirectoryRepository _repository;
//
//     public CreateDepartmentHandler(ILogger<CreateDepartmentHandler> logger,
//         IValidator<CreateDepartmentCommand> validator, IDirectoryRepository repository)
//     {
//         _logger = logger;
//         _validator = validator;
//         _repository = repository;
//     }
//
//
//     public async Task<Result<Guid, ErrorList>> Handle(CreateDepartmentCommand command,
//         CancellationToken cancellationToken)
//     {
//         // валидация input параметров
//         var validationResult = await _validator.ValidateAsync(command, cancellationToken);
//         if (validationResult.IsValid == false)
//             return validationResult.ToErrorList();
//
//
//         var isLocationResult =
//             await _repository.AllLocationsExistAsync(command.AddDepartmentRequest.Locations, cancellationToken);
//
//         if (!isLocationResult)
//             return Errors.General.NotFound().ToErrorList();
//
//         // создание VO
//         var name = DepartmentName.Create(command.AddDepartmentRequest.Name);
//
//         var identifier = Identifier.Create(command.AddDepartmentRequest.Identifier);
//
//         var parentId = command.AddDepartmentRequest.ParentId;
//
//         Result<Domain.Entities.Department, Error> createDepartmentResult;
//
//         if (parentId == null)
//             createDepartmentResult = Domain.Entities.Department.CreateParent(name.Value, identifier.Value, null);
//         else
//         {
//             var departmentId = DepartmentId.Create(parentId.Value);
//
//             var parent = await _repository.GetDepartmentById(departmentId.Value, cancellationToken);
//             if (parent.IsFailure)
//                 return parent.Error.ToErrorList();
//
//             createDepartmentResult =
//                 Domain.Entities.Department.CreateParent(name.Value, identifier.Value, parent.Value.Id);
//             if (createDepartmentResult.IsFailure)
//                 return createDepartmentResult.Error.ToErrorList();
//         }
//
//         var department = createDepartmentResult.Value;
//
//         var departmentLocations = command.AddDepartmentRequest.Locations
//             .Select(locationId => new DepartmentLocation(
//                 departmentId: department.Id,
//                 locationId: LocationId.Create(locationId).Value
//             ))
//             .ToList();
//     }
// }