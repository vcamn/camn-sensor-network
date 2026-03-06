namespace Fleet.Domain;

public class Site
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string SiteName { get; set; }

    public string AddressLine1 { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string PostalCode { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public string DeploymentStatus { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }

    public ICollection<Station> Stations { get; set; }
}