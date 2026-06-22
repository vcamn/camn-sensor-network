using Fleet.Api.DTOs.Site;

namespace Fleet.Api.Contracts;

public interface ISiteService
{
    Task<IEnumerable<SiteDto>> GetSitesAsync();

    Task<SiteDto?> GetSiteAsync(Guid id);

    Task<SiteDto> CreateSiteAsync(CreateSiteDto createSiteDto);

    Task UpdateSiteAsync(Guid id, SiteDto siteDto);

    Task DeleteSiteAsync(Guid id);

    Task<bool> SiteExistsAsync(Guid id);
}
