using System.ComponentModel.DataAnnotations;

namespace Fleet.Api.DTOs.Site;

public class SiteDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid SiteStatusId { get; set; }

    public string? SiteName { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public DateTime? InstalledAtUtc { get; set; }

    public DateTime? LastMaintenanceAtUtc { get; set; }

    public string? ContactName { get; set; }

    public string? ContactEmail { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }
}
