using Fleet.Api.Contracts;
using Fleet.Api.DTOs.Device;
using Fleet.Domain.Entities;
using Fleet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Fleet.Api.Services;

public class DeviceService(FleetDbContext context) : IDeviceService
{
    public async Task<IEnumerable<DeviceDto>> GetDevicesAsync()
    {
        return await context.Devices
            .Select(d => ToDto(d))
            .ToListAsync();
    }

    public async Task<DeviceDto?> GetDeviceAsync(Guid id)
    {
        var device = await context.Devices.FindAsync(id);
        return device is null ? null : ToDto(device);
    }

    public async Task<DeviceDto> CreateDeviceAsync(CreateDeviceDto createDeviceDto)
    {
        var station = await context.Stations.FindAsync(createDeviceDto.StationId)
            ?? throw new KeyNotFoundException($"Station with id '{createDeviceDto.StationId}' not found.");

        var deviceStatus = await GetDeviceStatusByCodeAsync(createDeviceDto.DeviceStatusCode)
            ?? throw new ValidationException($"Device status with code '{createDeviceDto.DeviceStatusCode}' not found.");

        var device = new Device
        {
            StationId = station.Id,
            DeviceStatusId = deviceStatus.Id,
            DeviceIdentifier = createDeviceDto.DeviceIdentifier,
            DeviceType = createDeviceDto.DeviceType,
            SerialNumber = createDeviceDto.SerialNumber,
            MacAddress = createDeviceDto.MacAddress,
            IoTHubDeviceId = createDeviceDto.IoTHubDeviceId,
            FirmwareVersion = createDeviceDto.FirmwareVersion
        };

        context.Devices.Add(device);
        await context.SaveChangesAsync();

        return ToDto(device);
    }

    public async Task UpdateDeviceAsync(Guid id, DeviceDto deviceDto)
    {
        var device = await context.Devices.FindAsync(id) ??
            throw new KeyNotFoundException($"Device with id {id} not found.");

        var deviceStatus = await GetDeviceStatusByCodeAsync(deviceDto.DeviceStatusCode) 
            ?? throw new ValidationException($"Device status with code '{deviceDto.DeviceStatusCode}' not found.");

        device.DeviceStatusId = deviceStatus.Id;
        device.DeviceIdentifier = deviceDto.DeviceIdentifier ?? device.DeviceIdentifier;
        device.DeviceType = deviceDto.DeviceType ?? device.DeviceType;
        device.SerialNumber = deviceDto.SerialNumber ?? device.SerialNumber;
        device.MacAddress = deviceDto.MacAddress ?? device.MacAddress;
        device.IoTHubDeviceId = deviceDto.IoTHubDeviceId ?? device.IoTHubDeviceId;
        device.FirmwareVersion = deviceDto.FirmwareVersion ?? device.FirmwareVersion;
        device.UpdatedAtUtc = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }

    public async Task DeleteDeviceAsync(Guid id)
    {
        var device = await context.Devices.FindAsync(id) ??
            throw new KeyNotFoundException($"Device with id {id} not found.");

        context.Devices.Remove(device);
        await context.SaveChangesAsync();
    }

    public async Task<bool> DeviceExistsAsync(Guid id)
    {
        return await context.Devices.AnyAsync(e => e.Id == id);
    }

    private async Task<DeviceStatus?> GetDeviceStatusByCodeAsync(string deviceStatusCode)
    {
        return await context.DeviceStatuses
            .FirstOrDefaultAsync(ds => ds.Code == deviceStatusCode);
    }

    private static DeviceDto ToDto(Device device)
    {
        return new DeviceDto
        {
            Id = device.Id,
            StationId = device.StationId,
            DeviceStatusCode = device.DeviceStatus.Code,
            DeviceIdentifier = device.DeviceIdentifier,
            DeviceType = device.DeviceType,
            SerialNumber = device.SerialNumber,
            MacAddress = device.MacAddress,
            IoTHubDeviceId = device.IoTHubDeviceId,
            FirmwareVersion = device.FirmwareVersion,
            LastActiveAtUtc = device.LastActiveAtUtc,
            CreatedAtUtc = device.CreatedAtUtc,
            UpdatedAtUtc = device.UpdatedAtUtc
        };
    }
}
