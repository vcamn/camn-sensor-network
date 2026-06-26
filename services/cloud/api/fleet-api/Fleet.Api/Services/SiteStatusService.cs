using Fleet.Api.Contracts;
using Fleet.Api.DTOs.Common;
using Fleet.Domain.Entities;
using Fleet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Api.Services;

public class SiteStatusService(FleetDbContext context) : ISiteStatusService
{
    public async Task<SystemStatusDto> CreateStatusAsync(SystemStatusDto statusDto)
    {
        var siteStatus = new SiteStatus
        {
            Id = statusDto.Id,
            Code = statusDto.Code,
            StatusName = statusDto.StatusName,
            Description = statusDto.Description,
            DisplayOrder = statusDto.DisplayOrder
        };

        context.SiteStatuses.Add(siteStatus);
        await context.SaveChangesAsync();

        return ToDto(siteStatus);
    }

    public async Task DeleteStatusAsync(Guid id)
    {
        // Network operations have net yet defined the process for deleting site statuses
        throw new NotImplementedException();
    }

    public async Task<SystemStatusDto?> GetStatusAsync(Guid id)
    {
        var siteStatus = await context.SiteStatuses.FindAsync(id);
        return siteStatus is null ? null : ToDto(siteStatus);
    }

    public async Task<IEnumerable<SystemStatusDto>> GetStatusesAsync()
    {
        return await context.SiteStatuses
            .Select(s => new SystemStatusDto
            {
                Id = s.Id,
                Code = s.Code,
                StatusName = s.StatusName,
                Description = s.Description,
                DisplayOrder = s.DisplayOrder
            })
            .ToListAsync();
    }

    public async Task UpdateStatusAsync(Guid id, SystemStatusDto statusDto)
    {
        var siteStatus = context.SiteStatuses.Find(id) ?? 
            throw new KeyNotFoundException($"SiteStatus with id {id} not found.");

        // Update the existing entity with the new values
        siteStatus.Code = statusDto.Code;
        siteStatus.StatusName = statusDto.StatusName;
        siteStatus.Description = statusDto.Description;
        siteStatus.DisplayOrder = statusDto.DisplayOrder;

        await context.SaveChangesAsync();
    }

    public async Task<bool> StatusExistsAsync(Guid id)
    {
        return await context.SiteStatuses.AnyAsync(s => s.Id == id);
    }

    private static SystemStatusDto ToDto(SiteStatus siteStatus)
    {
        return new SystemStatusDto
        {
            Id = siteStatus.Id,
            Code = siteStatus.Code,
            StatusName = siteStatus.StatusName,
            Description = siteStatus.Description,
            DisplayOrder = siteStatus.DisplayOrder
        };
    }
}
