using System;

namespace Fleet.Domain.Entities;

public class SensorIntegration
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid SensorId { get; set; }

    public Guid IntegrationTypeId { get; set; }

    public Guid IntegrationTemplateId { get; set; }

    public string ConfigJson { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }

    public Sensor Sensor { get; set; }

    public IntegrationType IntegrationType { get; set; }

    public IntegrationTemplate IntegrationTemplate { get; set; }
}
