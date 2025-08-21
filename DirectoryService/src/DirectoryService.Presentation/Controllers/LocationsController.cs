using DirectoryService.Application.Add.AddLocation;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Presentation.Controllers;
public class LocationsController : ApplicationController
{
    [HttpPost("api/locations")]
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

       return Ok(result.Value);
    }
}