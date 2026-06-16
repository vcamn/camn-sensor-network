using Fleet.Api.Contracts;
using Fleet.Api.DTOs.Common;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SensorStatusesController(ISensorStatusService sensorStatusService) : ControllerBase
{
    // GET: api/SensorStatuses
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SystemStatusDto>>> GetSensorStatuses()
    {
        var sensorStatuses = await sensorStatusService.GetStatusesAsync();
        return Ok(sensorStatuses);
    }

    // GET: api/SensorStatuses/5
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SystemStatusDto>> GetSensorStatus(Guid id)
    {
        var sensorStatusDto = await sensorStatusService.GetStatusAsync(id);
        if (sensorStatusDto == null)
        {
            return NotFound();
        }

        return Ok(sensorStatusDto);
    }

    // PUT: api/SensorStatuses/5
    [NonAction]
    [HttpPut("{id}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> PutSensorStatus(Guid id, SystemStatusDto sensorStatusDto)
    {
        return Ok();
    }

    // POST: api/SensorStatuses
    [NonAction]
    [HttpPost]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<ActionResult<SystemStatusDto>> PostSensorStatus(SystemStatusDto sensorStatusDto)
    {
        return Ok();
    }

    // DELETE: api/SensorStatuses/5
    [NonAction]
    [HttpDelete("{id}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> DeleteSensorStatus(Guid id)
    {
        return Ok();
    }
}
