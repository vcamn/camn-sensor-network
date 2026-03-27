using System;

namespace Fleet.Domain.Entities;

public class IntegrationType
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string TypeName { get; set; } = default!; // ["EDGE_DEVICE", "EXTERNAL_API", "MANUAL_UPLOAD", "Other"]

    public string? Description { get; set; }

    public int DisplayOrder { get; set; }

    public ICollection<IntegrationTemplate> IntegrationTemplates { get; set; } = [];
    public ICollection<SensorIntegration> SensorIntegrations { get; set; } = [];
}
