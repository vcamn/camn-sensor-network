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
    public async Task<ActionResult<IEnumerable<SiteStatus>>> GetSiteStatuses()
    {
        return await context.SiteStatuses.ToListAsync();
    }

    // GET: api/SiteStatus/5
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SiteStatus>> GetSiteStatus(Guid id)
    {
        var siteStatus = await context.SiteStatuses.FindAsync(id);

        if (siteStatus == null)
        {
            return NotFound();
        }

        return siteStatus;
    }

    // PUT: api/SiteStatus/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSiteStatus(Guid id, SiteStatus siteStatus)
    {
        if (id != siteStatus.Id)
        {
            return BadRequest();
        }

        context.Entry(siteStatus).State = EntityState.Modified;

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
    public async Task<ActionResult<SiteStatus>> PostSiteStatus(SiteStatus siteStatus)
    {
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
