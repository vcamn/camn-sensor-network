using Fleet.Api.DTOs.Common;
using Fleet.Domain.Entities;
using Fleet.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StationStatusController(FleetDbContext context) : ControllerBase
{

    // GET: api/StationStatus
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SystemStatusDto>>> GetStationStatuses()
    {
        var stationStatuses = await context.StationStatuses.Select(s => new SystemStatusDto
        {
            Id = s.Id,
            Code = s.Code,
            StatusName = s.StatusName,
            Description = s.Description,
            DisplayOrder = s.DisplayOrder
        }).ToListAsync();

        return Ok(stationStatuses);
    }

    // GET: api/StationStatus/5
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SystemStatusDto>> GetStationStatus(Guid id)
    {
        var stationStatus = await context.StationStatuses.FindAsync(id);

        if (stationStatus == null)
        {
            return NotFound();
        }

        var stationStatusDto = new SystemStatusDto
        {
            Id = stationStatus.Id,
            Code = stationStatus.Code,
            StatusName = stationStatus.StatusName,
            Description = stationStatus.Description,
            DisplayOrder = stationStatus.DisplayOrder
        };

        return Ok(stationStatusDto);
    }

    // PUT: api/StationStatus/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutStationStatus(Guid id, SystemStatusDto stationStatusDto)
    {
        if (id != stationStatusDto.Id)
        {
            return BadRequest();
        }

        var stationStatus = await context.StationStatuses.FindAsync(id);
        if (stationStatus == null)
        {
            return NotFound();
        }

        stationStatus.Code = stationStatusDto.Code;
        stationStatus.StatusName = stationStatusDto.StatusName;
        stationStatus.Description = stationStatusDto.Description;
        stationStatus.DisplayOrder = stationStatusDto.DisplayOrder;

        context.Entry(stationStatus).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await StationStatusExistsAsync(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/StationStatus
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<StationStatus>> PostStationStatus(SystemStatusDto stationStatusDto)
    {
        var exists = await context.StationStatuses.FindAsync(stationStatusDto.Id);
        if (exists != null)
        {
            return Conflict($"A StationStatus with ID {stationStatusDto.Id} already exists.");
        }

        var stationStatus = new StationStatus
        {
            Id = stationStatusDto.Id,
            Code = stationStatusDto.Code,
            StatusName = stationStatusDto.StatusName,
            Description = stationStatusDto.Description,
            DisplayOrder = stationStatusDto.DisplayOrder
        };

        context.StationStatuses.Add(stationStatus);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetStationStatus", new { id = stationStatus.Id }, stationStatus);
    }

    // DELETE: api/StationStatus/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStationStatus(Guid id)
    {
        var stationStatus = await context.StationStatuses.FindAsync(id);
        if (stationStatus == null)
        {
            return NotFound();
        }

        context.StationStatuses.Remove(stationStatus);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> StationStatusExistsAsync(Guid id)
    {
        return await context.StationStatuses.AnyAsync(e => e.Id == id);
    }
}
