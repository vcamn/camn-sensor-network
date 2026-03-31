using System;

namespace Fleet.Domain.Entities;

public class SensorStatus
{
    public Guid Id { get; private set; }

    public string Code { get; private set; } = default!;

    public string StatusName { get; private set; } = default!;

    public string? Description { get; private set; }
    
    public int DisplayOrder { get; private set; }

    public ICollection<Sensor> Sensors { get; private set; } = [];

    private SensorStatus() { } // For EF Core
}

/*
| status_name  |
| ------------ |
| Active       |
| Inactive     |
| Offline      |
| Provisioning |
| Maintenance  |
| Retired      |
*/