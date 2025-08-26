using DirectoryService.Application.Position;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;

public class PositionController : ApplicationController
{
    
    [HttpPost("api/departments")]
    public async Task<IActionResult> AddDepartment(
        [FromServices] AddPositionHandler handler,
        [FromBody] AddPositionRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddPositionCommand(request.Name, request.Description, request.DepartmentIds);
        
        var result =  await handler.Handle(command, cancellationToken);
        if(result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}