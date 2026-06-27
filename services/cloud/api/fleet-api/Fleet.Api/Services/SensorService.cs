using Fleet.Api.Contracts;
using Fleet.Api.DTOs.Sensor;
using Fleet.Domain.Entities;
using Fleet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Fleet.Api.Services;

/// <summary>
/// Service for managing sensor operations including CRUD operations with validation.
/// </summary>
public class SensorService(FleetDbContext context) : ISensorService
{
    /// <summary>
    /// Retrieves all sensors with their status information.
    /// </summary>
    /// <returns>A collection of all sensor DTOs.</returns>
    public async Task<IEnumerable<SensorDto>> GetSensorsAsync()
    {
        return await context.Sensors
            .Include(s => s.SensorType)
            .Include(s => s.Status)
            .Include(s => s.MeasurementUnit)
            .Include(s => s.Device)
            .Select(s => ToDto(s))
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves a specific sensor by its ID.
    /// </summary>
    /// <param name="id">The sensor ID.</param>
    /// <returns>The sensor DTO if found; otherwise null.</returns>
    public async Task<SensorDto?> GetSensorAsync(Guid id)
    {
        var sensor = await context.Sensors
            .Include(s => s.SensorType)
            .Include(s => s.Status)
            .Include(s => s.MeasurementUnit)
            .Include(s => s.Device)
            .FirstOrDefaultAsync(s => s.Id == id);
        return sensor is null ? null : ToDto(sensor);
    }

    /// <summary>
    /// Creates a new sensor with validation of related entities.
    /// </summary>
    /// <param name="createSensorDto">The sensor creation data.</param>
    /// <returns>The created sensor DTO.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when a referenced entity is not found.</exception>
    /// <exception cref="ValidationException">Thrown when validation fails.</exception>
    public async Task<SensorDto> CreateSensorAsync(CreateSensorDto createSensorDto)
    {
        var sensorType = await GetSensorTypeByTypeName(createSensorDto.SensorTypeName)
            ?? throw new KeyNotFoundException($"Sensor type with type name '{createSensorDto.SensorTypeName}' not found.");

        var station = await context.Stations.FindAsync(createSensorDto.StationId)
            ?? throw new KeyNotFoundException($"Station with id '{createSensorDto.StationId}' not found.");

        var sensorStatus = await GetSensorStatusByCodeAsync(createSensorDto.SensorStatusCode)
            ?? throw new ValidationException($"Sensor status with code '{createSensorDto.SensorStatusCode}' not found.");

        var measurementUnit = await GetMeasurementUnitByCodeAsync(createSensorDto.MeasurementUnitCode)
            ?? throw new KeyNotFoundException($"Measurement unit with code '{createSensorDto.MeasurementUnitCode}' not found.");

        Device? device = null;
        if (createSensorDto.DeviceIdentifier is not null)
        {
            device = await GetDeviceByIdentifierAsync(createSensorDto.DeviceIdentifier)
                ?? throw new KeyNotFoundException($"Device with identifier '{createSensorDto.DeviceIdentifier}' not found.");
        }

        var sensor = new Sensor
        {
            SensorTypeId = sensorType.Id,
            StationId = station.Id,
            DeviceId = device?.Id,
            SensorStatusId = sensorStatus.Id,
            MeasurementUnitId = measurementUnit.Id,
            SensorIdentifier = createSensorDto.SensorIdentifier,
            CalibrationDate = createSensorDto.CalibrationDate
        };

        context.Sensors.Add(sensor);
        await context.SaveChangesAsync();

        return ToDto(sensor);
    }

    /// <summary>
    /// Updates an existing sensor with validation of related entities.
    /// </summary>
    /// <param name="id">The sensor ID.</param>
    /// <param name="sensorDto">The sensor update data.</param>
    /// <exception cref="KeyNotFoundException">Thrown when the sensor or a referenced entity is not found.</exception>
    /// <exception cref="ValidationException">Thrown when validation fails.</exception>
    public async Task UpdateSensorAsync(Guid id, SensorDto sensorDto)
    {
        var sensor = await context.Sensors.FindAsync(id)
            ?? throw new KeyNotFoundException($"Sensor with id '{id}' not found.");

        var sensorType = await GetSensorTypeByTypeName(sensorDto.SensorTypeName)
            ?? throw new KeyNotFoundException($"Sensor type with type name '{sensorDto.SensorTypeName}' not found.");

        var station = await context.Stations.FindAsync(sensorDto.StationId)
            ?? throw new KeyNotFoundException($"Station with id '{sensorDto.StationId}' not found.");

        var sensorStatus = await GetSensorStatusByCodeAsync(sensorDto.SensorStatusCode)
            ?? throw new ValidationException($"Sensor status with code '{sensorDto.SensorStatusCode}' not found.");

        var measurementUnit = await GetMeasurementUnitByCodeAsync(sensorDto.MeasurementUnitCode)
            ?? throw new KeyNotFoundException($"Measurement unit with code '{sensorDto.MeasurementUnitCode}' not found.");

        Device? device = null;
        if (sensorDto.DeviceIdentifier is not null)
        {
            device = await GetDeviceByIdentifierAsync(sensorDto.DeviceIdentifier)
                ?? throw new KeyNotFoundException($"Device with identifier '{sensorDto.DeviceIdentifier}' not found.");
        }

        sensor.SensorTypeId = sensorType.Id;
        sensor.StationId = station.Id;
        sensor.DeviceId = device?.Id;
        sensor.SensorStatusId = sensorStatus.Id;
        sensor.MeasurementUnitId = measurementUnit.Id;
        sensor.SensorIdentifier = sensorDto.SensorIdentifier;
        sensor.CalibrationDate = sensorDto.CalibrationDate;
        sensor.UpdatedAtUtc = DateTime.UtcNow;

        context.Sensors.Update(sensor);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a sensor by its ID.
    /// </summary>
    /// <param name="id">The sensor ID.</param>
    /// <exception cref="KeyNotFoundException">Thrown when the sensor is not found.</exception>
    public async Task DeleteSensorAsync(Guid id)
    {
        var sensor = await context.Sensors.FindAsync(id)
            ?? throw new KeyNotFoundException($"Sensor with id '{id}' not found.");

        context.Sensors.Remove(sensor);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Checks if a sensor exists by its ID.
    /// </summary>
    /// <param name="id">The sensor ID.</param>
    /// <returns>True if the sensor exists; otherwise false.</returns>
    public async Task<bool> SensorExistsAsync(Guid id)
    {
        return await context.Sensors.AnyAsync(s => s.Id == id);
    }

    private async Task<SensorStatus?> GetSensorStatusByCodeAsync(string code)
    {
        return await context.SensorStatuses
            .FirstOrDefaultAsync(s => s.Code == code);
    }

    private async Task<SensorType?> GetSensorTypeByTypeName(string sensorTypeName)
    {
        return await context.SensorTypes
            .FirstOrDefaultAsync(s => s.TypeName == sensorTypeName);
    }

    private async Task<MeasurementUnit?> GetMeasurementUnitByCodeAsync(string code)
    {
        return await context.MeasurementUnits
            .FirstOrDefaultAsync(m => m.Code == code);
    }

    private async Task<Device?> GetDeviceByIdentifierAsync(string deviceIdentifier)
    {
        return await context.Devices
            .FirstOrDefaultAsync(d => d.DeviceIdentifier == deviceIdentifier);
    }

    private static SensorDto ToDto(Sensor sensor)
    {
        return new SensorDto
        {
            Id = sensor.Id,
            SensorTypeName = sensor.SensorType.TypeName,
            StationId = sensor.StationId,
            DeviceIdentifier = sensor.Device?.DeviceIdentifier,
            SensorStatusCode = sensor.Status?.Code ?? string.Empty,
            MeasurementUnitCode = sensor.MeasurementUnit.Code,
            SensorIdentifier = sensor.SensorIdentifier,
            CalibrationDate = sensor.CalibrationDate,
            CreatedAtUtc = sensor.CreatedAtUtc,
            UpdatedAtUtc = sensor.UpdatedAtUtc
        };
    }
}
