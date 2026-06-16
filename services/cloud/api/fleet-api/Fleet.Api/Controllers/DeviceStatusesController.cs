using Fleet.Api.Contracts;
using Fleet.Api.DTOs.Common;
using Fleet.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeviceStatusesController(IDeviceStatusService deviceStatusService) : ControllerBase
{
    // GET: api/DeviceStatuses
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SystemStatusDto>>> GetDeviceStatuses()
    {
        var statuses = await deviceStatusService.GetStatusesAsync();
        return Ok(statuses);
    }

    // GET: api/DeviceStatuses/5
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SystemStatusDto>> GetDeviceStatus(Guid id)
    {
        var deviceStatus = await deviceStatusService.GetStatusAsync(id);
        if (deviceStatus == null)
        {
            return NotFound();
        }

        return Ok(deviceStatus);
    }

    // PUT: api/DeviceStatuses/5
    [NonAction]
    [HttpPut("{id}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> PutDeviceStatus(Guid id, DeviceStatus deviceStatus)
    {
        return Ok();
    }

    // POST: api/DeviceStatuses
    [HttpPost]
    [NonAction]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<ActionResult<DeviceStatus>> PostDeviceStatus(DeviceStatus deviceStatus)
    {
        return Ok();
    }

    // DELETE: api/DeviceStatuses/5
    [NonAction]
    [HttpDelete("{id:guid}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> DeleteDeviceStatus(Guid id)
    {
        return Ok();
    }
}
