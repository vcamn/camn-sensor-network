using Fleet.Api.Contracts;
using Fleet.Api.DTOs.SensorType;
using Fleet.Domain.Entities;
using Fleet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fleet.Api.Services;

public class SensorTypeService(FleetDbContext context) : ISensorTypeService
{
    public async Task<IEnumerable<SensorTypeDto>> GetSensorTypesAsync()
    {
        return await context.SensorTypes.Select(s => ToDto(s)).ToListAsync();
    }

    public async Task<SensorTypeDto?> GetSensorTypeAsync(Guid id)
    {
        var sensorType = await context.SensorTypes.FindAsync(id);
        return sensorType is null ? null : ToDto(sensorType);
    }

    public async Task UpdateSensorTypeAsync(Guid id, SensorTypeDto sensorTypeDto)
    {
        var sensorType = await context.SensorTypes.FindAsync(id) ??
            throw new KeyNotFoundException($"SensorType with ID {id} not found.");

        // Update the sensorType entity with the values from sensorTypeDto
        sensorType.TypeName = sensorTypeDto.TypeName;
        sensorType.Description = sensorTypeDto.Description;
        sensorType.UpdatedAtUtc = DateTime.UtcNow;
        if (sensorTypeDto.DisplayOrder.HasValue)
        {
            sensorType.DisplayOrder = sensorTypeDto.DisplayOrder.Value;
        }

        await context.SaveChangesAsync();
    }

    public async Task<SensorTypeDto> CreateSensorTypeAsync(CreateSensorTypeDto createSensorTypeDto)
    {
        SensorType newSensorType = new()
        {
            Id = Guid.NewGuid(),
            TypeName = createSensorTypeDto.TypeName,
            Description = createSensorTypeDto.Description,
            DisplayOrder = createSensorTypeDto.DisplayOrder,
            IsActive = createSensorTypeDto.IsActive
        };

        context.SensorTypes.Add(newSensorType);
        await context.SaveChangesAsync();
        return ToDto(newSensorType);
    }

    public async Task DeactivateSensorTypeAsync(Guid id)
    {
        await SetActiveStatus(id, false);
    }

    public async Task ActivateSensorTypeAsync(Guid id)
    {
        await SetActiveStatus(id, true);
    }

    public async Task<bool> SensorTypeExistsAsync(Guid id)
    {
        return await context.SensorTypes.AnyAsync(e => e.Id == id);
    }

    private async Task SetActiveStatus(Guid id, bool isActive)
    {
        var sensorType = await context.SensorTypes.FindAsync(id) ??
            throw new KeyNotFoundException($"SensorType with ID {id} not found.");

        sensorType.IsActive = isActive;
        sensorType.UpdatedAtUtc = DateTime.UtcNow;
        await context.SaveChangesAsync();
    }

    private static SensorTypeDto ToDto(SensorType sensorType)
    {
        return new SensorTypeDto
        {
            Id = sensorType.Id,
            TypeName = sensorType.TypeName,
            Description = sensorType.Description,
            DisplayOrder = sensorType.DisplayOrder,
            IsActive = sensorType.IsActive,
            CreatedAtUtc = sensorType.CreatedAtUtc,
            UpdatedAtUtc = sensorType.UpdatedAtUtc
        };
    }

}
