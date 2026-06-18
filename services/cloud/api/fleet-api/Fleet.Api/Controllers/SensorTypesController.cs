using Asp.Versioning;
using Fleet.Api.Contracts;
using Fleet.Api.DTOs.SensorType;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class SensorTypesController(ISensorTypeService sensorTypeService) : ControllerBase
{
    // GET: api/SensorTypes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SensorTypeDto>>> GetSensorTypes()
    {
        var items = await sensorTypeService.GetSensorTypesAsync();
        return Ok(items);
    }

    // GET: api/SensorTypes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<SensorTypeDto>> GetSensorType(Guid id)
    {
        var sensorTypeDto = await sensorTypeService.GetSensorTypeAsync(id);
        if (sensorTypeDto == null)
        {
            return NotFound();
        }

        return Ok(sensorTypeDto);
    }

    // PUT: api/SensorTypes/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSensorType(Guid id, SensorTypeDto sensorTypeDto)
    {
        if (id != sensorTypeDto.Id)
        {
            return BadRequest();
        }

        try
        {
            await sensorTypeService.UpdateSensorTypeAsync(id, sensorTypeDto);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    // POST: api/SensorTypes
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<SensorTypeDto>> PostSensorType(CreateSensorTypeDto createSensorTypeDto)
    {
        var created = await sensorTypeService.CreateSensorTypeAsync(createSensorTypeDto);
        return CreatedAtAction(nameof(GetSensorType), new { id = created.Id }, created);
    }

    // POST: api/SensorTypes/5/deactivate
    [HttpPost("{id:guid}/deactivate")]
    public async Task<IActionResult> DeactivateSensorType(Guid id)
    {
        try
        {
            await sensorTypeService.DeactivateSensorTypeAsync(id);

        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    // POST: api/SensorTypes/5/activate
    [HttpPost("{id:guid}/activate")]
    public async Task<IActionResult> ActivateSensorType(Guid id)
    {
        try
        {
            await sensorTypeService.ActivateSensorTypeAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}
