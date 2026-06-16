using Fleet.Api.Contracts;
using Fleet.Api.DTOs.Common;
using Fleet.Domain.Entities;
using Fleet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Api.Services;

public class SensorStatusService(FleetDbContext context) : ISensorStatusService
{
    public async Task<SystemStatusDto> CreateStatusAsync(SystemStatusDto statusDto)
    {
        // A status change for sensors would be a deployment/versioning event, not a normal application operation and
        // is not expected to be created by users, so this method is not implemented.
        throw new NotImplementedException();
    }

    public async Task DeleteStatusAsync(Guid id)
    {
        // A status change for sensors would be a deployment/versioning event, not a normal application operation and
        // is not expected to be created by users, so this method is not implemented.
        throw new NotImplementedException();
    }

    public async Task<SystemStatusDto?> GetStatusAsync(Guid id)
    {
        var sensorStatus = await context.SensorStatuses.FindAsync(id);
        return sensorStatus is null ? null : ToDto(sensorStatus);
    }

    public async Task<IEnumerable<SystemStatusDto>> GetStatusesAsync()
    {
        return await context.SensorStatuses
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
        // A status change for sensors would be a deployment/versioning event, not a normal application operation and
        // is not expected to be created by users, so this method is not implemented.
        throw new NotImplementedException();
    }

    public async Task<bool> StatusExistsAsync(Guid id)
    {
        return await context.SensorStatuses.AnyAsync(s => s.Id == id);
    }

    private static SystemStatusDto ToDto(SensorStatus sensorStatus)
    {
        return new SystemStatusDto
        {
            Id = sensorStatus.Id,
            Code = sensorStatus.Code,
            StatusName = sensorStatus.StatusName,
            Description = sensorStatus.Description,
            DisplayOrder = sensorStatus.DisplayOrder
        };
    }
}
