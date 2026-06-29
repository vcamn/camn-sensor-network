using Fleet.Api.Contracts;
using Fleet.Api.DTOs.Site;
using Fleet.Api.DTOs.Station;
using Fleet.Domain.Entities;
using Fleet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Fleet.Api.Services;

public class SitesService(FleetDbContext context) : ISiteService
{
    public async Task<IEnumerable<SiteDto>> GetSitesAsync()
    {
        return await context.Sites
            .Include(s => s.Status)
            .Select(s => ToDto(s))
            .ToListAsync();
    }

    public async Task<SiteDto?> GetSiteAsync(Guid id)
    {
        var site = await context.Sites
            .Include(s => s.Status)
            .FirstOrDefaultAsync(s => s.Id == id);

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
        var siteStatus = await GetSiteStatusByCodeAsync(createSiteDto.SiteStatusCode) ??
            throw new ValidationException($"Site status with code {createSiteDto.SiteStatusCode} not found.");

        var site = new Site
        {
            SiteStatusId = siteStatus.Id,
            SiteName = createSiteDto.SiteName,
            SiteCode = createSiteDto.SiteCode,
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
            Status = siteStatus,
        };

        context.Sites.Add(site);
        await context.SaveChangesAsync();

        return ToDto(site);
    }

    public async Task UpdateSiteAsync(Guid id, SiteDto siteDto)
    {
        var site = await context.Sites.FindAsync(id) ??
            throw new KeyNotFoundException($"Site with id {id} not found.");

        var siteStatus = await GetSiteStatusByCodeAsync(siteDto.SiteStatusCode) ??
            throw new ValidationException($"Site status with code {siteDto.SiteStatusCode} not found.");

        site.SiteStatusId = siteStatus.Id;
        site.SiteName = siteDto.SiteName ?? site.SiteName;
        site.SiteCode = siteDto.SiteCode ?? site.SiteCode;
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
        // Network operations have net yet defined the process for deleting sites
        throw new NotImplementedException();
    }

    public async Task<bool> SiteExistsAsync(Guid id)
    {
        return await context.Sites.AnyAsync(s => s.Id == id);
    }

    public async Task<SiteStatus?> GetSiteStatusByCodeAsync(string siteStatusCode)
    {
        var siteStatus = await context.SiteStatuses
            .FirstOrDefaultAsync(ss => ss.Code == siteStatusCode);

        return siteStatus;
    }

    private static SiteDto ToDto(Site s)
    {
        return new SiteDto
        {
            Id = s.Id,
            SiteStatusCode = s.Status.Code,
            SiteName = s.SiteName,
            SiteCode = s.SiteCode,
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
