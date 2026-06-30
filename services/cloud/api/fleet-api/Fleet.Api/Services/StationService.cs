using Fleet.Api.Contracts;
using Fleet.Api.DTOs.Device;
using Fleet.Api.DTOs.Sensor;
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
        };

        context.Stations.Add(station);
        await context.SaveChangesAsync();

        return ToDto(station);
    }

    public async Task DeleteStationAsync(Guid id)
    {
        // Network operations have net yet defined the process for deleting stations
        throw new NotImplementedException();
    }

    public async Task<StationDto?> GetStationAsync(Guid id)
    {
        var station = await context.Stations
            .Include(s => s.Status)
            .FirstOrDefaultAsync(s => s.Id == id);
        return station is null ? null : ToDto(station);
    }

    public async Task<IEnumerable<StationDto>> GetStationsAsync()
    {
        return await context.Stations
            .Include(s => s.Status)
            .Select(s => ToDto(s))
            .ToListAsync();
    }

    public async Task<IEnumerable<DeviceDto>> GetStationDevicesAsync(Guid stationId)
    {
        var station = await context.Stations.FindAsync(stationId);
        if (station is null)
        {
            throw new KeyNotFoundException($"Station with id {stationId} not found.");
        }

        return await context.Devices
            .Where(d => d.StationId == stationId)
            .Include(d => d.DeviceStatus)
            .Select(d => DeviceService.ToDto(d))
            .ToListAsync();
    }

    public async Task<IEnumerable<SensorDto>> GetStationSensorsAsync(Guid stationId)
    {
        var station = await context.Stations.FindAsync(stationId);
        if (station is null)
        {
            throw new KeyNotFoundException($"Station with id {stationId} not found.");
        }

        return await context.Sensors
            .Where(s => s.StationId == stationId)
            .Include(s => s.SensorType)
            .Include(s => s.Status)
            .Include(s => s.MeasurementUnit)
            .Include(s => s.Device)
            .Select(s => SensorService.ToDto(s))
            .ToListAsync();
    }

    public async Task UpdateStationAsync(Guid id, StationDto stationDto)
    {
        var station = await context.Stations.FindAsync(id) ??
            throw new KeyNotFoundException($"Station with id {id} not found.");

        var stationStatus = await GetStationStatusByCodeAsync(stationDto.StationStatusCode) ??
            throw new ValidationException($"Unknown station status with code '{stationDto.StationStatusCode}'.");

        var site = await context.Sites.FindAsync(stationDto.SiteId) ??
            throw new KeyNotFoundException($"Unknown site with id '{stationDto.SiteId}'.");

        if (station.SiteId != stationDto.SiteId)
        {
            var message = $"Operation does not support Request. Station {station.Id} is associated with site {station.SiteId}, but the request attempted to change it to site {stationDto.SiteId}.";
            throw new ValidationException(message);
        }

        station.StationStatusId = stationStatus.Id;
        station.StationCode = stationDto.StationCode ?? station.StationCode;
        station.Description = stationDto.Description ?? station.Description;
        station.SiteId = site.Id;
        station.UpdatedAtUtc = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }

    public async Task<bool> StationExistsAsync(Guid id)
    {
        return await context.Stations.AnyAsync(s => s.Id == id);
    }

    public async Task<StationStatus?> GetStationStatusByCodeAsync(string stationStatusCode)
    {
        return await context.StationStatuses
            .FirstOrDefaultAsync(ss => ss.Code == stationStatusCode);
    }

    public static StationDto ToDto(Station s)
    {
        return new StationDto
        {
            Id = s.Id,
            SiteId = s.SiteId,
            StationStatusCode = s.Status.Code,
            StationCode = s.StationCode,
            Description = s.Description,
            CreatedAtUtc = s.CreatedAtUtc,
            UpdatedAtUtc = s.UpdatedAtUtc
        };
    }
}
