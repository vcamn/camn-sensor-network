using Fleet.Api.Contracts;
using Fleet.Api.DTOs.Station;
using Fleet.Domain.Entities;
using Fleet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Fleet.Api.Services;

public class StationService(FleetDbContext context) : IStationService
{
    public async Task<StationDto> CreateStationAsync(CreateStationDto createStationDto)
    {
        var stationStatus = await GetStationStatusByCodeAsync(createStationDto.StationStatusCode) 
            ?? throw new ValidationException($"Unknown station status with code '{createStationDto.StationStatusCode}'.");

        var site = await context.Sites.FindAsync(createStationDto.SiteId)
            ?? throw new KeyNotFoundException($"Unknown site with id '{createStationDto.SiteId}'.");

        var station = new Station
        {
            SiteId = createStationDto.SiteId,
            StationStatusId = stationStatus.Id,
            StationCode = createStationDto.StationCode,
            Description = createStationDto.Description,
            Site = site,
            Status = stationStatus,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };

        context.Stations.Add(station);
        await context.SaveChangesAsync();

        return ToDto(station);
    }

    public async Task DeleteStationAsync(Guid id)
    {
        var station = await context.Stations.FindAsync(id) ??
            throw new KeyNotFoundException($"Station with id {id} not found.");

        context.Stations.Remove(station);
        await context.SaveChangesAsync();
    }

    public async Task<StationDto?> GetStationAsync(Guid id)
    {
        var station = await context.Stations.FindAsync(id);
        return station is null ? null : ToDto(station);
    }

    public async Task<IEnumerable<StationDto>> GetStationsAsync()
    {
        return await context.Stations
            .Select(s => ToDto(s))
            .ToListAsync();
    }

    public async Task UpdateStationAsync(Guid id, StationDto stationDto)
    {
        var station = await context.Stations.FindAsync(id) ??
            throw new KeyNotFoundException($"Station with id {id} not found.");

        station.StationStatusId = stationDto.StationStatusId;
        station.StationCode = stationDto.StationCode ?? station.StationCode;
        station.Description = stationDto.Description ?? station.Description;
        station.SiteId = stationDto.SiteId;
        station.UpdatedAtUtc = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }

    public async Task<bool> StationExistsAsync(Guid id)
    {
        return await context.Stations.AnyAsync(s => s.Id == id);
    }

    private async Task<StationStatus?> GetStationStatusByCodeAsync(string stationStatusCode)
    {
        return await context.StationStatuses
            .FirstOrDefaultAsync(ss => ss.Code == stationStatusCode);
    }

    private static StationDto ToDto(Station s)
    {
        return new StationDto
        {
            Id = s.Id,
            SiteId = s.SiteId,
            StationStatusId = s.StationStatusId,
            StationCode = s.StationCode,
            Description = s.Description,
            CreatedAtUtc = s.CreatedAtUtc,
            UpdatedAtUtc = s.UpdatedAtUtc
        };
    }
}
