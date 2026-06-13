using Fleet.Api.DTOs.Common;
using Fleet.Domain.Entities;
using Fleet.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SiteStatusController(FleetDbContext context) : ControllerBase
{

    // GET: api/SiteStatus
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SystemStatusDto>>> GetSiteStatuses()
    {
        var siteStatuses = await context.SiteStatuses.Select(s => new SystemStatusDto
        {
            Id = s.Id,
            Code = s.Code,
            StatusName = s.StatusName,
            Description = s.Description,
            DisplayOrder = s.DisplayOrder
        }).ToListAsync();

        return Ok(siteStatuses);
    }

    // GET: api/SiteStatus/5
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SystemStatusDto>> GetSiteStatus(Guid id)
    {
        var siteStatus = await context.SiteStatuses.FindAsync(id);

        if (siteStatus == null)
        {
            return NotFound();
        }

        var siteStatusDto = new SystemStatusDto
        {
            Id = siteStatus.Id,
            Code = siteStatus.Code,
            StatusName = siteStatus.StatusName,
            Description = siteStatus.Description,
            DisplayOrder = siteStatus.DisplayOrder
        };

        return Ok(siteStatusDto);
    }

    // PUT: api/SiteStatus/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSiteStatus(Guid id, SystemStatusDto siteStatusDto)
    {
        if (id != siteStatusDto.Id)
        {
            return BadRequest();
        }

        // Check if the record exists before updating
        var siteStatus = await context.SiteStatuses.FindAsync(id);
        if (siteStatus == null)
        {
            return NotFound();
        }

        // Update the existing entity with the new values
        siteStatus.Code = siteStatusDto.Code;
        siteStatus.StatusName = siteStatusDto.StatusName;
        siteStatus.Description = siteStatusDto.Description;
        siteStatus.DisplayOrder = siteStatusDto.DisplayOrder;

        // Mark the entity as modified
        context.Entry(siteStatus).State = EntityState.Modified;

        // Save changes with concurrency handling
        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (! await SiteStatusExistsAsync(id))
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

    // POST: api/SiteStatus
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<SiteStatus>> PostSiteStatus(SystemStatusDto siteStatusDto)
    {
        // TODO: Check if a record with the same ID already exists
        var siteStatusExists = await context.SiteStatuses.FindAsync(siteStatusDto.Id);
        if (siteStatusExists != null)
        {
            return Conflict($"A SiteStatus with ID {siteStatusDto.Id} already exists.");
        }

        var siteStatus = new SiteStatus
        {
            Id = siteStatusDto.Id,
            Code = siteStatusDto.Code,
            StatusName = siteStatusDto.StatusName,
            Description = siteStatusDto.Description,
            DisplayOrder = siteStatusDto.DisplayOrder
        };

        context.SiteStatuses.Add(siteStatus);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetSiteStatus", new { id = siteStatus.Id }, siteStatus);
    }

    // DELETE: api/SiteStatus/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSiteStatus(Guid id)
    {
        var siteStatus = await context.SiteStatuses.FindAsync(id);
        if (siteStatus == null)
        {
            return NotFound();
        }

        context.SiteStatuses.Remove(siteStatus);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> SiteStatusExistsAsync(Guid id)
    {
        return await context.SiteStatuses.AnyAsync(e => e.Id == id);
    }
}
