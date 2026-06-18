using Fleet.Api.DTOs.SensorType;

namespace Fleet.Api.Contracts;

public interface ISensorTypeService
{
    Task<IEnumerable<SensorTypeDto>> GetSensorTypesAsync();

    Task<SensorTypeDto?> GetSensorTypeAsync(Guid id);

    Task UpdateSensorTypeAsync(Guid id, SensorTypeDto sensorTypeDto);

    Task<SensorTypeDto> CreateSensorTypeAsync(CreateSensorTypeDto createSensorTypeDto);

    Task DeactivateSensorTypeAsync(Guid id);

    Task ActivateSensorTypeAsync(Guid id);

    Task<bool> SensorTypeExistsAsync(Guid id);
}
