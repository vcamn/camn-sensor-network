using Asp.Versioning;
using Fleet.Api.Contracts;
using Fleet.Api.DTOs.Device;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fleet.Api.Controllers;


[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class DevicesController(IDeviceService deviceService) : ControllerBase
{
    // GET: api/v1/Devices
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DeviceDto>>> GetDevices()
    {
        var devices = await deviceService.GetDevicesAsync();
        return Ok(devices);
    }

    // GET: api/v1/Devices/5
    [HttpGet("{id}")]
    public async Task<ActionResult<DeviceDto>> GetDevice(Guid id)
    {
        var device = await deviceService.GetDeviceAsync(id);
        if (device == null)
        {
            return NotFound();
        }

        return Ok(device);
    }

    // PUT: api/v1/Devices/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDevice(Guid id, DeviceDto deviceDto)
    {
        if (id != deviceDto.Id)
        {
            return BadRequest();
        }

        try
        {
            await deviceService.UpdateDeviceAsync(id, deviceDto);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ValidationException ex) 
        {
            return BadRequest(ex.Message);
        }

        return NoContent();
    }

    // POST: api/Devices
    [HttpPost]
    public async Task<ActionResult<DeviceDto>> PostDevice(CreateDeviceDto createDeviceDto)
    {
        var device = await deviceService.CreateDeviceAsync(createDeviceDto);
        return CreatedAtAction("GetDevice", new { id = device.Id }, device);
    }

    // DELETE: api/Devices/5
    [HttpDelete("{id}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> DeleteDevice(Guid id)
    {
        return Ok();
    }
}
