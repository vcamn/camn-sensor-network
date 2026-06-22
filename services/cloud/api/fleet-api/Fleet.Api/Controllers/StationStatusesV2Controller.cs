using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Api.Controllers;

[ApiController]
[ApiVersion("2.0", Deprecated = true)]
[Route("api/v{version:apiVersion}/stationstatuses")]
public class StationStatusesV2Controller : ControllerBase
{
    [HttpGet]
    public IActionResult GetStationStatuses()
    {
        return Ok(new 
        { 
            Controller = "StationStatusesV2Controller",
            Version = "2.0",
            Message = "This is an API versioning example for the controller StationStatuses. Version 2.0 - Example for future reference." 
        });
    }
}
