using System;
using Fleet.Domain.Constants;

namespace Fleet.Infrastructure.Persistence.SeedData;

public static class DeviceStatusSeed
{
    public static readonly (Guid Id, string Code, string Name, string Description, int DisplayOrder)[] Data = new[]
    {
        (DeviceStatusIds.Active, "ACTIVE", "Active", "Device is operating normally", 1),
        (DeviceStatusIds.Inactive, "INACTIVE", "Inactive", "Device should be operating but is not (problem)", 2),
        (DeviceStatusIds.Disabled, "DISABLED", "Disabled", "Device intentionally turned off", 3),
        (DeviceStatusIds.Offline, "OFFLINE", "Offline", "Device is operating offline", 4),
        (DeviceStatusIds.Error, "ERROR", "Error", "Device is in fault state", 5),
        (DeviceStatusIds.Provisioning, "PROVISIONING", "Provisioning", "Device is being provisioned (set up and configured)", 6),
        (DeviceStatusIds.Maintenance, "MAINTENANCE", "Maintenance", "Device under maintenance", 7),
        (DeviceStatusIds.Decommissioned, "DECOMMISSIONED", "Decommissioned", "Device permanently retired", 8)
    };
}
