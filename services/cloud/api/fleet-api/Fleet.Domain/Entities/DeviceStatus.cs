using System;

namespace Fleet.Domain.Entities;

public class DeviceStatus
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string StatusName { get; set; } = default!;

    public string? Description { get; set; }

    public int DisplayOrder { get; set; }
    
    public ICollection<Device> Devices { get; set; } = [];
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