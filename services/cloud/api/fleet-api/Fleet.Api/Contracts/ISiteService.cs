using Fleet.Api.DTOs.Site;
using Fleet.Api.DTOs.Station;

namespace Fleet.Api.Contracts;

public interface ISiteService
{
    Task<IEnumerable<SiteDto>> GetSitesAsync();

    Task<SiteDto?> GetSiteAsync(Guid id);

    Task<IEnumerable<StationDto>> GetSiteStationsAsync(Guid siteId);

    Task<SiteDto> CreateSiteAsync(CreateSiteDto createSiteDto);

    Task UpdateSiteAsync(Guid id, SiteDto siteDto);

    Task DeleteSiteAsync(Guid id);

    Task<bool> SiteExistsAsync(Guid id);
}
