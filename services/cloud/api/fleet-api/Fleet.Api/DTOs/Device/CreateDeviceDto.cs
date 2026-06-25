using System.ComponentModel.DataAnnotations;

namespace Fleet.Api.DTOs.Device;

public class CreateDeviceDto
{
    [Required]
    public Guid StationId { get; set; }

    [Required]
    public required string DeviceStatusCode { get; set; }

    public required string DeviceIdentifier { get; set; }

    public required string DeviceType { get; set; }

    public string? SerialNumber { get; set; }

    public string? MacAddress { get; set; }

    public string? IoTHubDeviceId { get; set; }

    public string? FirmwareVersion { get; set; }
}
