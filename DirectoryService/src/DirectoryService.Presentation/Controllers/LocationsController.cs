using DirectoryService.Application.Location.AddLocation;
using DirectoryService.Application.Location.GetLocationWithPagination;
using DirectoryService.Application.Location.UpdateLocation;
using DirectoryService.Contracts.Requests;
using DirectoryService.Domain.ValueObjects.LocationVO;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;
public class LocationsController : ApplicationController
{
    [HttpPost("/api/locations")]
    public async Task<IActionResult> AddLocation(
        [FromServices] AddLocationsHandler handler,
        [FromBody] AddLocationsRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddLocationCommand(
            request.Name, 
            request.Address, 
            request.TimeZone);
        
       var result =  await handler.Handle(command, cancellationToken);
       if(result.IsFailure)
           return BadRequest(result.Error);

       return Ok(result.Value);
    }

    [HttpGet("/api/locations")]
    public async Task<ActionResult<PaginationLocationResponse>> GetAllLocations(
        [FromQuery] GetLocationWithPaginationRequest request,
        
        [FromServices] GetLocationWIthPaginationHandler handler,
        CancellationToken cancellationToken)
    {
        var locations = await handler.Handle(request, cancellationToken);
        
        return Ok(locations);
    }


    [HttpPut("/api/departments/{departmentId}/locations")]

    public async Task<IActionResult> UpdateDepartmentLocations(
        [FromRoute] Guid departmentId,
        [FromServices] UpdateLocationHandler handler,
        [FromBody] UpdateLocationRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateLocationCommand(departmentId, request.LocationIds);
        
        var result = await handler.Handle(command, cancellationToken);
        
        if(result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
}