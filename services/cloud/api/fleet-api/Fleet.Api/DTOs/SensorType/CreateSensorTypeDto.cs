using System.ComponentModel.DataAnnotations;

namespace Fleet.Api.DTOs.SensorType;

public class CreateSensorTypeDto
{
    [Required]
    [MaxLength(32)]
    public required string TypeName { get; set; }

    [Required]
    [MaxLength(256)]
    public required string Description { get; set; }

    public int DisplayOrder { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;
}
