using System;
using Fleet.Domain.Constants;

namespace Fleet.Infrastructure.Persistence.SeedData;

public static class StationStatusSeed
{
    public static readonly (Guid Id, string Code, string Name, string Description, int DisplayOrder)[] Data = new[]
    {
        (StationStatusIds.Active, "ACTIVE", "Active", "Station operational", 1),
        (StationStatusIds.Inactive, "INACTIVE", "Inactive", "Station should be operating but is not (problem)", 2),
        (StationStatusIds.Offline, "OFFLINE", "Offline", "Station is operating offline", 3),
        (StationStatusIds.Maintenance, "MAINTENANCE", "Maintenance", "Under maintenance", 4),
        (StationStatusIds.Decommissioned, "DECOMMISSIONED", "Decommissioned", "Station permanently retired", 5)
    };
}
