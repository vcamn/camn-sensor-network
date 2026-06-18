namespace Fleet.Api.DTOs.SensorType;

public class SensorTypeDto
{
    public Guid Id { get; set; }

    public required string TypeName { get; set; }

    public required string Description { get; set; }

    public int? DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }
}
