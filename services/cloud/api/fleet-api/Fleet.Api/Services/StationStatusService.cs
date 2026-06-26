using Fleet.Api.Contracts;
using Fleet.Api.DTOs.Common;
using Fleet.Domain.Entities;
using Fleet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Api.Services;

public class StationStatusService(FleetDbContext context) : IStationStatusService
{
    public async Task<SystemStatusDto> CreateStatusAsync(SystemStatusDto statusDto)
    {
        var stationStatus = new StationStatus
        {
            Id = statusDto.Id,
            Code = statusDto.Code,
            StatusName = statusDto.StatusName,
            Description = statusDto.Description,
            DisplayOrder = statusDto.DisplayOrder
        };

        context.StationStatuses.Add(stationStatus);
        await context.SaveChangesAsync();

        return ToDto(stationStatus);
    }

    public async Task DeleteStatusAsync(Guid id)
    {
        // Network operations have net yet defined the process for deleting station statuses
        throw new NotImplementedException();
    }

    public async Task<SystemStatusDto?> GetStatusAsync(Guid id)
    {
        var stationStatus = await context.StationStatuses.FindAsync(id);
        return stationStatus is null ? null : ToDto(stationStatus);
    }

    public async Task<IEnumerable<SystemStatusDto>> GetStatusesAsync()
    {
        return await context.StationStatuses
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
        var stationStatus = await context.StationStatuses.FindAsync(id)
            ?? throw new KeyNotFoundException($"StationStatus with id {id} not found.");

        stationStatus.Code = statusDto.Code;
        stationStatus.StatusName = statusDto.StatusName;
        stationStatus.Description = statusDto.Description;
        stationStatus.DisplayOrder = statusDto.DisplayOrder;

        await context.SaveChangesAsync();
    }

    public async Task<bool> StatusExistsAsync(Guid id)
    {
        return await context.StationStatuses.AnyAsync(s => s.Id == id);
    }

    private static SystemStatusDto ToDto(StationStatus stationStatus)
    {
        return new SystemStatusDto
        {
            Id = stationStatus.Id,
            Code = stationStatus.Code,
            StatusName = stationStatus.StatusName,
            Description = stationStatus.Description,
            DisplayOrder = stationStatus.DisplayOrder
        };
    }
}