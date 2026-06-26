using Asp.Versioning;
using Fleet.Api.Contracts;
using Fleet.Api.DTOs.Station;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Fleet.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class StationsController(IStationService stationService) : ControllerBase
{
    // GET: api/v1/Stations
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StationDto>>> GetStations()
    {
        var stations = await stationService.GetStationsAsync();
        return Ok(stations);
    }

    // GET: api/v1/Stations/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<StationDto>> GetStation(Guid id)
    {
        var station = await stationService.GetStationAsync(id);
        if (station == null)
        {
            return NotFound();
        }

        return Ok(station);
    }

    // PUT: api/v1/Stations/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> PutStation(Guid id, StationDto stationDto)
    {
        if (id != stationDto.Id)
        {
            return BadRequest();
        }

        try
        {
            await stationService.UpdateStationAsync(id, stationDto);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    // POST: api/v1/Stations
    [HttpPost]
    public async Task<ActionResult<StationDto>> PostStation(CreateStationDto createStation)
    {
        try
        {
            var created = await stationService.CreateStationAsync(createStation);
            return CreatedAtAction(nameof(GetStation), new { id = created.Id }, created);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // DELETE: api/v1/Stations/{id}
    [HttpDelete("{id:guid}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> DeleteStation(Guid id)
    {
        return Ok();
    }
}
