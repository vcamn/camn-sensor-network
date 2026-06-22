using System.ComponentModel.DataAnnotations;

namespace Fleet.Api.DTOs.Site;

public class CreateSiteDto
{
    [Required]
    [MaxLength(32)]
    public required string SiteStatusCode { get; set; }

    [Required]
    [MaxLength(128)]
    public required string SiteName { get; set; }

    [MaxLength(128)]
    public string? AddressLine1 { get; set; }

    [MaxLength(128)]
    public string? AddressLine2 { get; set; }

    [Required]
    [MaxLength(64)]
    public string? City { get; set; }

    [MaxLength(64)]
    public string? State { get; set; }

    [Required]
    [MaxLength(16)]
    public string? PostalCode { get; set; }

    [MaxLength(64)]
    public string? Country { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public DateTime? InstalledAtUtc { get; set; }

    public DateTime? LastMaintenanceAtUtc { get; set; }

    [MaxLength(64)]
    public string? ContactName { get; set; }

    [EmailAddress]
    public string? ContactEmail { get; set; }
}
