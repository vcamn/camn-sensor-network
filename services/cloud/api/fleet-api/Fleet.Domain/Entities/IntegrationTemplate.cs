using System;

namespace Fleet.Domain.Entities;

public class IntegrationTemplate
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid SensorTypeId { get; set; }

    public Guid IntegrationTypeId { get; set; }

    public string Description { get; set; }

    public string DefaultConfigJson { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }

    public IntegrationType IntegrationType { get; set; } 

    public SensorType SensorType { get; set; }

    public ICollection<SensorIntegration> SensorIntegrations { get; set; } = [];
}
