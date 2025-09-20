using CSharpFunctionalExtensions;
using Dapper;
using DirectoryService.Application.Database;
using DirectoryService.Application.Interfaces;
using DirectoryService.Contracts.Dtos;
using DirectoryService.Contracts.Errors;
using DirectoryService.Domain.ValueObjects.DepartmentVO;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.Location.ReplacementLocation;

public class ReplacementDepartmentHandler(
    IDepartmentRepository departmentRepository, 
    ILocationRepository locationRepository,
    ILogger<ReplacementDepartmentHandler> logger, 
    IValidator<ReplacementDepartmentCommand> validator, 
    ITransactionManager transactionManager,
    ISqlConnectionFactory sqlConnectionFactory)
{
    public async Task<Result<Guid, ErrorList>> Handle(
        ReplacementDepartmentCommand command,
        CancellationToken cancellationToken)
    {
        using var connection = sqlConnectionFactory.Create();
        
        var validateResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validateResult.IsValid)
            return Errors.General.ValueIsInvalid().ToErrorList();
        
        // id департамента, который будет перенесен.
        var departmentId = DepartmentId.Create(command.DepartmentId);

        var transactionResult = await transactionManager.BeginTransactionAsync(cancellationToken);
        
        using var transaction = transactionResult.Value;
        
        // получаем департамент из бд и блокируем.
        var getDepartmentResult = await departmentRepository.GetByIdWithLockAsync(departmentId.Value.Value, cancellationToken);
        if (getDepartmentResult.IsFailure)
        {
            transaction.Rollback(cancellationToken);
            return Errors.General.ValueIsInvalid().ToErrorList();
        }
        
        Domain.Entities.Department? parentDepartment = null;
        if (command.ParentId is not null)
        {
            var parentDepartmentId = DepartmentId.Create(command.ParentId.Value);
            
            var isDescendantsResult = await departmentRepository.IsDescendant(
                departmentId.Value, 
                parentDepartmentId.Value);
            
            if (isDescendantsResult.IsFailure)
            {
                transaction.Rollback(cancellationToken);
                return Errors.General.ValueIsInvalid().ToErrorList();
            }
            var parentDepartmentResult = await departmentRepository.GetByIdWithLockAsync(parentDepartmentId.Value.Value, cancellationToken);
            if(parentDepartmentResult.IsFailure)
                return Errors.General.ValueIsInvalid().ToErrorList();
            
            parentDepartment = parentDepartmentResult.Value;
        }

        var oldPath = getDepartmentResult.Value.Path;
        
        var setParentResult = getDepartmentResult.Value.SetParent(parentDepartment);
        if (setParentResult.IsFailure)
        {
            transaction.Rollback(cancellationToken);
            return Errors.General.ValueIsInvalid().ToErrorList();
        }
        
        var updateDepartmentResult = await transactionManager.SaveChangesAsync(cancellationToken);
        if (updateDepartmentResult.IsFailure)
            return updateDepartmentResult.Error.ToErrorList();
        
        var lockDescendantsResult = await departmentRepository.LockDescendants(getDepartmentResult.Value.Path);
        if (lockDescendantsResult.IsFailure)
        {
            transaction.Rollback(cancellationToken);
            return Errors.General.ValueIsInvalid().ToErrorList();
        }
        
        var updateDescendantDepartments = await departmentRepository.MoveDepartments(
            getDepartmentResult.Value,
            oldPath);
        
        if (updateDescendantDepartments.IsFailure)
            return updateDescendantDepartments.Error.ToErrorList();

        var updateDepartmentsResult = transaction.Commit(cancellationToken);
        if (updateDepartmentsResult.IsFailure)
            return updateDepartmentsResult.Error.ToErrorList();
        
        logger.LogInformation("Updated parent department for department with id {departmentId}", command.DepartmentId);
        
        return getDepartmentResult.Value.Id.Value;
    }
}