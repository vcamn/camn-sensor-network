using System.ComponentModel.DataAnnotations;

namespace Fleet.Api.DTOs.Device;

public class DeviceDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid StationId { get; set; }

    [Required]
    public required string DeviceStatusCode { get; set; }

    [Required]
    public required string DeviceIdentifier { get; set; }

    [Required]
    public required string DeviceType { get; set; }

    public string? SerialNumber { get; set; }

    public string? MacAddress { get; set; }

    public string? IoTHubDeviceId { get; set; }

    public string? FirmwareVersion { get; set; }

    public DateTime? LastActiveAtUtc { get; set; }

    public DateTime? CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }
}
