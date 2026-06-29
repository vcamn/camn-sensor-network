using System.Diagnostics.Metrics;

namespace Fleet.Domain.Entities
{
    public class Site
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid SiteStatusId { get; set; }

        public required string SiteName { get; set; }

        public required string SiteCode { get; set; }

        public required string AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        public required string City { get; set; }

        public required string State { get; set; }

        public required string PostalCode { get; set; }

        public required string Country { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public DateTime? InstalledAtUtc { get; set; }

        public DateTime? LastMaintenanceAtUtc { get; set; }

        public required string ContactName { get; set; }

        public required string ContactEmail { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public DateTime UpdatedAtUtc { get; set; }

        public required SiteStatus Status { get; set; }

        public ICollection<Station> Stations { get; set; } = [];
    }
}