using System.ComponentModel.DataAnnotations;

namespace Fleet.Api.DTOs.Station;

public class CreateStationDto
{
    [Required]
    [MaxLength(32)]
    public required string StationStatusCode { get; set; }

    [Required]
    public required Guid SiteId { get; set; }

    [Required]
    [MaxLength(128)]
    public required string StationCode { get; set; }

    [MaxLength(2048)]
    public string? Description { get; set; }
}
