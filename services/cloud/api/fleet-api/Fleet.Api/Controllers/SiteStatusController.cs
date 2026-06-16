using Fleet.Api.Contracts;
using Fleet.Api.DTOs.Common;
using Fleet.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SiteStatusController(ISiteStatusService siteStatusService) : ControllerBase
{

    // GET: api/SiteStatus
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SystemStatusDto>>> GetSiteStatuses()
    {
        var siteStatuses =  await siteStatusService.GetStatusesAsync();
        return Ok(siteStatuses);
    }

    // GET: api/SiteStatus/5
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SystemStatusDto>> GetSiteStatus(Guid id)
    {
        var siteStatusDto = await siteStatusService.GetStatusAsync(id);
        if (siteStatusDto == null)
        {
            return NotFound();
        }

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

        try
        {
            // Update the existing entity with the new values
            await siteStatusService.UpdateStatusAsync(id, siteStatusDto);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    // POST: api/SiteStatus
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<SiteStatus>> PostSiteStatus(SystemStatusDto siteStatusDto)
    {
        if (await siteStatusService.StatusExistsAsync(siteStatusDto.Id))
        {
            return Conflict($"A SiteStatus with ID {siteStatusDto.Id} already exists.");
        }

        var created = await siteStatusService.CreateStatusAsync(siteStatusDto);

        return CreatedAtAction("GetSiteStatus", new { id = created.Id }, created);
    }

    // DELETE: api/SiteStatus/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSiteStatus(Guid id)
    {
        try
        {
            await siteStatusService.DeleteStatusAsync(id);

        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}
