using Asp.Versioning;
using Fleet.Api.Contracts;
using Fleet.Api.DTOs.Site;
using Fleet.Api.DTOs.Station;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SitesController(ISiteService siteService) : ControllerBase
    {
        // GET: api/v1/Sites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SiteDto>>> GetSites()
        {
            var sites = await siteService.GetSitesAsync();
            return Ok(sites);
        }

        // GET: api/v1/Sites/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SiteDto>> GetSite(Guid id)
        {
            var site = await siteService.GetSiteAsync(id);
            if (site == null)
            {
                return NotFound();
            }

            return Ok(site);
        }

        // GET: api/v1/Sites/{id}/stations
        [HttpGet("{id:guid}/stations")]
        public async Task<ActionResult<IEnumerable<StationDto>>> GetSiteStations(Guid id)
        {
            try
            {
                var stations = await siteService.GetSiteStationsAsync(id);
                if (stations == null)
                {
                    return NotFound();
                }

                return Ok(stations);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/v1/Sites
        [HttpPost]
        public async Task<ActionResult<SiteDto>> PostSite(CreateSiteDto createSite)
        {
            var created = await siteService.CreateSiteAsync(createSite);
            return CreatedAtAction(nameof(GetSite), new { id = created.Id }, created);
        }

        // PUT: api/v1/Sites/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> PutSite(Guid id, SiteDto siteDto)
        {
            if (id != siteDto.Id)
            {
                return BadRequest();
            }

            try
            {
                await siteService.UpdateSiteAsync(id, siteDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            return NoContent();
        }

        // DELETE: api/v1/Sites/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteSite(Guid id)
        {
            try
            {
                await siteService.DeleteSiteAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
