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

    [Required]
    [MaxLength(32)]
    public required string SiteCode { get; set; }

    [Required]
    [MaxLength(128)]
    public required string AddressLine1 { get; set; }

    [MaxLength(128)]
    public string? AddressLine2 { get; set; }

    [Required]
    [MaxLength(64)]
    public required string City { get; set; }

    [MaxLength(64)]
    public required string State { get; set; }

    [Required]
    [MaxLength(16)]
    public required string PostalCode { get; set; }

    [MaxLength(64)]
    public required string Country { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public DateTime? InstalledAtUtc { get; set; }

    public DateTime? LastMaintenanceAtUtc { get; set; }

    [MaxLength(64)]
    public required string ContactName { get; set; }

    [EmailAddress]
    public required string ContactEmail { get; set; }
}
