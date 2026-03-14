using System;

namespace Fleet.Domain.Entities;

public class SensorStatus
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string StatusName { get; set; } = default!;

    public string? Description { get; set; }
    
    public int DisplayOrder { get; set; }

    public ICollection<Sensor> Sensors { get; set; } = [];
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