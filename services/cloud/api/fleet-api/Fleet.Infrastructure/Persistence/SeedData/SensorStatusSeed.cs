using System;
using Fleet.Domain.Constants;

namespace Fleet.Infrastructure.Persistence.SeedData;

public static class SensorStatusSeed
{
    public static readonly (Guid Id, string Code, string Name, string Description, int DisplayOrder)[] Data = new[]
    {
        (SensorStatusIds.Active, "ACTIVE", "Active", "Sensor is operating normally", 1),
        (SensorStatusIds.Inactive, "INACTIVE", "Inactive", "Sensor should be operating but is not (problem)", 2),
        (SensorStatusIds.Disabled, "DISABLED", "Disabled", "Sensor intentionally turned off", 3),
        (SensorStatusIds.Offline, "OFFLINE", "Offline", "Sensor is operating offline", 4),
        (SensorStatusIds.Error, "ERROR", "Error", "Sensor is in fault state", 5),
        (SensorStatusIds.Provisioning, "PROVISIONING", "Provisioning", "Sensor is being provisioned (set up and configured)", 6),
        (SensorStatusIds.Maintenance, "MAINTENANCE", "Maintenance", "Sensor under maintenance", 7),
        (SensorStatusIds.Decommissioned, "DECOMMISSIONED", "Decommissioned", "Sensor permanently retired", 8)
    };
}
