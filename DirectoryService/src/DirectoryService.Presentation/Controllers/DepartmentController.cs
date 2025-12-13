using DirectoryService.Application.Department;
using DirectoryService.Application.Department.Query;
using DirectoryService.Application.Department.Query.GetChildDepartments;
using DirectoryService.Application.Department.Query.GetRootDepartment;
using DirectoryService.Application.Department.Query.GetTopDepartment;
using DirectoryService.Application.Location.AddLocation;
using DirectoryService.Application.Location.GetLocationWithPagination;
using DirectoryService.Application.Location.ReplacementLocation;
using DirectoryService.Contracts.Dtos;
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
    
    [HttpGet]
    public async Task<ActionResult<DepartmentTopDto>> GetAllLocations(
        [FromServices] GetTopDepartmentHandler handler,
        CancellationToken cancellationToken)
    {
        var topDepartments = await handler.Handle(cancellationToken);
        
        return Ok(topDepartments);
    }
    
    [Route("/api/departments/roots")]
    [HttpGet]
    public async Task<ActionResult<DepartmentTopDto>> GetRootDepartment(
        [FromQuery]  GetRootDepartmentRequest request,
        [FromServices] GetRootDepartmentHandler handler,
        CancellationToken cancellationToken)
    {
        
        var rootDepartment = await handler.GetRootDepartments(request, cancellationToken);
        
        return Ok(rootDepartment);
    }
    
    [Route("/api/departments/children")]
    [HttpGet]
    public async Task<ActionResult<DepartmentTopDto>> GetChildDepartments(
        [FromQuery]  GetChildDepartmentRequest request,
        [FromServices] GetChildDepartmentHandler handler,
        CancellationToken cancellationToken)
    {
        
        var rootDepartment = await handler.GetChildDepartment(request, cancellationToken);
        
        return Ok(rootDepartment);
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