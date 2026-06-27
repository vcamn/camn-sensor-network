using System.ComponentModel.DataAnnotations;

namespace Fleet.Api.DTOs.Sensor;

public class SensorDto
{
    [Required(ErrorMessage = "Sensor ID is required.")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Sensor type name is required.")]
    public required string SensorTypeName { get; set; }

    [Required(ErrorMessage = "Station ID is required.")]
    public Guid StationId { get; set; }

    public string? DeviceIdentifier { get; set; }

    [Required(ErrorMessage = "Sensor status code is required.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Sensor status code must be between 1 and 50 characters.")]
    public required string SensorStatusCode { get; set; }

    [Required(ErrorMessage = "Measurement unit code is required.")]
    [StringLength(16, MinimumLength = 3, ErrorMessage = "Measurement unit code must be between 1 and 16 characters.")]
    public required string MeasurementUnitCode { get; set; }

    [Required(ErrorMessage = "Sensor identifier is required.")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Sensor identifier must be between 1 and 255 characters.")]
    public required string SensorIdentifier { get; set; }

    public DateTime? CalibrationDate { get; set; }

    public DateTime? CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }
}
