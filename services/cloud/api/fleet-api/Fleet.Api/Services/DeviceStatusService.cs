using Fleet.Api.Contracts;
using Fleet.Api.DTOs.Common;
using Fleet.Domain.Entities;
using Fleet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Api.Services;

public class DeviceStatusService(FleetDbContext context) : IDeviceStatusService
{
    public async Task<SystemStatusDto> CreateStatusAsync(SystemStatusDto deviceStatus)
    {
        // A status change for devices would be a deployment/versioning event, not a normal application operation and
        // is not expected to be created by users, so this method is not implemented.
        throw new NotImplementedException();
    }

    public async Task DeleteStatusAsync(Guid id)
    {
        // A status change for devices would be a deployment/versioning event, not a normal application operation and
        // is not expected to be created by users, so this method is not implemented.
        throw new NotImplementedException();
    }

    public async Task<SystemStatusDto?> GetStatusAsync(Guid id)
    {
        var deviceStatus = await context.DeviceStatuses.FindAsync(id);
        return deviceStatus is null ? null : ToDto(deviceStatus);
    }

    public async Task<IEnumerable<SystemStatusDto>> GetStatusesAsync()
    {
        return await context.DeviceStatuses
            .Select(d => new SystemStatusDto
            {
                Id = d.Id,
                Code = d.Code,
                StatusName = d.StatusName,
                Description = d.Description,
                DisplayOrder = d.DisplayOrder
            })
            .ToListAsync();
    }

    public async Task UpdateStatusAsync(Guid id, SystemStatusDto statusDto)
    {
        // A status change for devices would be a deployment/versioning event, not a normal application operation and
        // is not expected to be created by users, so this method is not implemented.
        throw new NotImplementedException();
    }

    public async Task<bool> StatusExistsAsync(Guid id)
    {
        return await context.DeviceStatuses.AnyAsync(d => d.Id == id);
    }

    private static SystemStatusDto ToDto(DeviceStatus deviceStatus)
    {
        return new SystemStatusDto
        {
            Id = deviceStatus.Id,
            Code = deviceStatus.Code,
            StatusName = deviceStatus.StatusName,
            Description = deviceStatus.Description,
            DisplayOrder = deviceStatus.DisplayOrder
        };
    }
}
