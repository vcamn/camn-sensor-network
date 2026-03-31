using System;

namespace Fleet.Domain.Entities;

public class DeviceStatus
{
    public Guid Id { get; private set; }

    public string Code { get; private set; } = default!;

    public string StatusName { get; private set; } = default!;

    public string? Description { get; private set; }

    public int DisplayOrder { get; private set; }

    public ICollection<Device> Devices { get; private set; } = [];

    private DeviceStatus() { } // For EF Core
}

/*
| status_name  |
| ------------ |
| Active       |
| Offline      |
| Provisioning |
| Maintenance  |
| Retired      |
*/