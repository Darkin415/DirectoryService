using DirectoryService.Application.Department;
using DirectoryService.Application.Location.AddLocation;
using DirectoryService.Application.Location.ReplacementLocation;
using DirectoryService.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;

public class DepartmentController : ApplicationController
{
    
    [HttpPost("api/departments")]
    public async Task<IActionResult> AddDepartment(
        [FromServices] CreateDepartmentHandler handler,
        [FromBody] AddDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateDepartmentCommand(request.Name, request.Identifier, request.ParentId, request.Locations);
        
        var result =  await handler.Handle(command, cancellationToken);
        if(result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
    
    [HttpPut("api/departments/{departmentId}/parent")]
    public async Task<IActionResult> ReplacementDepartment(
        [FromRoute] Guid departmentId,
        [FromBody] Guid? parentId,
        [FromServices] ReplacementDepartmentHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new ReplacementDepartmentCommand(parentId, departmentId);
        
        var result =  await handler.Handle(command, cancellationToken);
        if(result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}