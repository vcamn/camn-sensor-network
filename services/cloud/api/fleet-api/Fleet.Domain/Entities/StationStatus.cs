using System;

namespace Fleet.Domain.Entities;

public class StationStatus
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string StatusName { get; set; } = default!;

    public string? Description { get; set; }

    public int DisplayOrder { get; set; }

    public ICollection<Station> Stations { get; set; } = [];
}

/*
| status_name    |
| -------------- |
| Active         |
| Inactive        |
| Maintenance    |
| Decommissioned |
*/