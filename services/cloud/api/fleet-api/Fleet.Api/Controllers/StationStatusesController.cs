using Asp.Versioning;
using Fleet.Api.Contracts;
using Fleet.Api.DTOs.Common;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class StationStatusesController(IStationStatusService stationStatusService) : ControllerBase
{

    // GET: api/StationStatuses
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SystemStatusDto>>> GetStationStatuses()
    {
        var stationStatuses = await stationStatusService.GetStatusesAsync();
        return Ok(stationStatuses);
    }

    // GET: api/StationStatuses/5
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SystemStatusDto>> GetStationStatus(Guid id)
    {
        var stationStatusDto = await stationStatusService.GetStatusAsync(id);
        if (stationStatusDto == null)
        {
            return NotFound();
        }

        return Ok(stationStatusDto);
    }

    // PUT: api/StationStatuses/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutStationStatus(Guid id, SystemStatusDto stationStatusDto)
    {
        if (id != stationStatusDto.Id)
        {
            return BadRequest();
        }

        try
        {
            await stationStatusService.UpdateStatusAsync(id, stationStatusDto);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    // POST: api/StationStatuses
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<SystemStatusDto>> PostStationStatus(SystemStatusDto stationStatusDto)
    {
        if (await stationStatusService.StatusExistsAsync(stationStatusDto.Id))
        {
            return Conflict($"A StationStatus with ID {stationStatusDto.Id} already exists.");
        }

        var created = await stationStatusService.CreateStatusAsync(stationStatusDto);

        return CreatedAtAction(nameof(GetStationStatus), new { id = created.Id }, created);
    }

    // DELETE: api/StationStatuses/5
    [HttpDelete("{id}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> DeleteStationStatus(Guid id)
    {
        return Ok();
    }
}
