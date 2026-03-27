using System.Diagnostics.Metrics;

namespace Fleet.Domain.Entities
{
    public class Site
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid SiteStatusId { get; set; }

        public string SiteName { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public DateTime? InstalledAtUtc { get; set; }

        public DateTime? LastMaintenanceAtUtc { get; set; }

        public string ContactName { get; set; }

        public string ContactEmail { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public DateTime UpdatedAtUtc { get; set; }

        public SiteStatus Status { get; set; }

        public ICollection<Station> Stations { get; set; } = [];
    }
}