using System.ComponentModel.DataAnnotations;

namespace Fleet.Api.DTOs.Station;

public class StationDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid SiteId { get; set; }

    [Required]
    public Guid StationStatusId { get; set; }

    public string? StationCode { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }
}
