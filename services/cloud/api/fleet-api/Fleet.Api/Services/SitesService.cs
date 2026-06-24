using Fleet.Api.Contracts;
using Fleet.Api.DTOs.Site;
using Fleet.Api.DTOs.Station;
using Fleet.Domain.Entities;
using Fleet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Api.Services;

public class SitesService(FleetDbContext context) : ISiteService
{
    public async Task<IEnumerable<SiteDto>> GetSitesAsync()
    {
        return await context.Sites
            .Select(s => ToDto(s))
            .ToListAsync();
    }

    public async Task<SiteDto?> GetSiteAsync(Guid id)
    {
        var site = await context.Sites.FindAsync(id);
        return site is null ? null : ToDto(site);
    }

    public async Task<IEnumerable<StationDto>> GetSiteStationsAsync(Guid siteId)
    {
        var site = await context.Sites.FindAsync(siteId);
        if (site is null)
        {
            throw new KeyNotFoundException($"Site with id {siteId} not found.");
        }

        return await context.Stations
            .Where(station => station.SiteId == siteId)
            .Select(station => StationService.ToDto(station))
            .ToListAsync();
    }

    public async Task<SiteDto> CreateSiteAsync(CreateSiteDto createSiteDto)
    {
        var siteStatus = await GetSiteStatusByCodeAsync(createSiteDto.SiteStatusCode);
        var site = new Site
        {
            SiteStatusId = siteStatus.Id,
            SiteName = createSiteDto.SiteName,
            AddressLine1 = createSiteDto.AddressLine1,
            AddressLine2 = createSiteDto.AddressLine2,
            City = createSiteDto.City,
            State = createSiteDto.State,
            PostalCode = createSiteDto.PostalCode,
            Country = createSiteDto.Country,
            Latitude = createSiteDto.Latitude,
            Longitude = createSiteDto.Longitude,
            InstalledAtUtc = createSiteDto.InstalledAtUtc,
            LastMaintenanceAtUtc = createSiteDto.LastMaintenanceAtUtc,
            ContactName = createSiteDto.ContactName,
            ContactEmail = createSiteDto.ContactEmail,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };

        context.Sites.Add(site);
        await context.SaveChangesAsync();

        return ToDto(site);
    }

    public async Task UpdateSiteAsync(Guid id, SiteDto siteDto)
    {
        var site = await context.Sites.FindAsync(id) ??
            throw new KeyNotFoundException($"Site with id {id} not found.");

        site.SiteStatusId = siteDto.SiteStatusId;
        site.SiteName = siteDto.SiteName ?? site.SiteName;
        site.AddressLine1 = siteDto.AddressLine1 ?? site.AddressLine1;
        site.AddressLine2 = siteDto.AddressLine2 ?? site.AddressLine2;
        site.City = siteDto.City ?? site.City;
        site.State = siteDto.State ?? site.State;
        site.PostalCode = siteDto.PostalCode ?? site.PostalCode;
        site.Country = siteDto.Country ?? site.Country;
        site.Latitude = siteDto.Latitude;
        site.Longitude = siteDto.Longitude;
        site.InstalledAtUtc = siteDto.InstalledAtUtc;
        site.LastMaintenanceAtUtc = siteDto.LastMaintenanceAtUtc;
        site.ContactName = siteDto.ContactName ?? site.ContactName;
        site.ContactEmail = siteDto.ContactEmail ?? site.ContactEmail;
        site.UpdatedAtUtc = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }

    public async Task DeleteSiteAsync(Guid id)
    {
        var site = await context.Sites.FindAsync(id) ??
            throw new KeyNotFoundException($"Site with id {id} not found.");

        context.Sites.Remove(site);
        await context.SaveChangesAsync();
    }

    public async Task<bool> SiteExistsAsync(Guid id)
    {
        return await context.Sites.AnyAsync(s => s.Id == id);
    }

    private async Task<SiteStatus> GetSiteStatusByCodeAsync(string siteStatusCode)
    {
        var siteStatus = await context.SiteStatuses
            .FirstOrDefaultAsync(ss => ss.Code == siteStatusCode);

        if (siteStatus is null)
        {
            throw new KeyNotFoundException($"Site status with code {siteStatusCode} not found.");
        }

        return siteStatus;
    }

    private static SiteDto ToDto(Site s)
    {
        return new SiteDto
        {
            Id = s.Id,
            SiteStatusId = s.SiteStatusId,
            SiteName = s.SiteName,
            AddressLine1 = s.AddressLine1,
            AddressLine2 = s.AddressLine2,
            City = s.City,
            State = s.State,
            PostalCode = s.PostalCode,
            Country = s.Country,
            Latitude = s.Latitude,
            Longitude = s.Longitude,
            InstalledAtUtc = s.InstalledAtUtc,
            LastMaintenanceAtUtc = s.LastMaintenanceAtUtc,
            ContactName = s.ContactName,
            ContactEmail = s.ContactEmail,
            CreatedAtUtc = s.CreatedAtUtc,
            UpdatedAtUtc = s.UpdatedAtUtc
        };
    }
}
