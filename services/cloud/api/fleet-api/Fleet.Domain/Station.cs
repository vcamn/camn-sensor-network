using System;

namespace Fleet.Domain;

public class Station
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid SiteId { get; set; }

    public string StationCode { get; set; }

    public string DeploymentStatus { get; set; }

    public DateTime? InstalledAtUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }

    public Site Site { get; set; }

    public ICollection<Device> Devices { get; set; }
}