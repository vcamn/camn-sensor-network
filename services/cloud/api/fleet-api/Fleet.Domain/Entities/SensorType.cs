using System;

namespace Fleet.Domain.Entities;

public class SensorType
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string TypeName { get; set; } // ["AethLabs", "AggieAir", "LTwind", "PurpleAir", "Other"]

    public string Description { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }

    public ICollection<Sensor> Sensors { get; set; } = [];
    
    public ICollection<IntegrationTemplate> IntegrationTemplates { get; set; } = [];
}
