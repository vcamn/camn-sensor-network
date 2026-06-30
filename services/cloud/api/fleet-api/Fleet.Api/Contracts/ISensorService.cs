using Fleet.Api.DTOs.Sensor;
using Fleet.Domain.Entities;

namespace Fleet.Api.Contracts;

/// <summary>
/// Service interface for managing sensor operations.
/// </summary>
public interface ISensorService
{
    /// <summary>
    /// Retrieves all sensors.
    /// </summary>
    /// <returns>A collection of all sensor DTOs.</returns>
    Task<IEnumerable<SensorDto>> GetSensorsAsync();

    /// <summary>
    /// Retrieves a specific sensor by its ID.
    /// </summary>
    /// <param name="id">The sensor ID.</param>
    /// <returns>The sensor DTO if found; otherwise null.</returns>
    Task<SensorDto?> GetSensorAsync(Guid id);

    /// <summary>
    /// Creates a new sensor.
    /// </summary>
    /// <param name="createSensorDto">The sensor creation data.</param>
    /// <returns>The created sensor DTO.</returns>
    Task<SensorDto> CreateSensorAsync(CreateSensorDto createSensorDto);

    /// <summary>
    /// Updates an existing sensor.
    /// </summary>
    /// <param name="id">The sensor ID.</param>
    /// <param name="sensorDto">The sensor update data.</param>
    Task UpdateSensorAsync(Guid id, SensorDto sensorDto);

    /// <summary>
    /// Deletes a sensor by its ID.
    /// </summary>
    /// <param name="id">The sensor ID.</param>
    Task DeleteSensorAsync(Guid id);

    /// <summary>
    /// Checks if a sensor exists by its ID.
    /// </summary>
    /// <param name="id">The sensor ID.</param>
    /// <returns>True if the sensor exists; otherwise false.</returns>
    Task<bool> SensorExistsAsync(Guid id);
    Task<SensorStatus?> GetSensorStatusByCodeAsync(string code);
    Task<SensorType?> GetSensorTypeByTypeName(string sensorTypeName);
    Task<MeasurementUnit?> GetMeasurementUnitByCodeAsync(string code);
    Task<Device?> GetDeviceByIdentifierAsync(string deviceIdentifier);
}
