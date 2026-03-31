using System;

namespace Fleet.Domain.Entities;

public class SiteStatus
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Code { get; set; } = default!;

    public string StatusName { get; set; } = default!;

    public string? Description { get; set; }
    
    public int DisplayOrder { get; set; }

    public ICollection<Site> Sites { get; set; } = [];
}

/*
| status_name | meaning                           |
| ----------- | --------------------------------- |
| Proposed    | candidate monitoring location     |
| Approved    | approved for installation         |
| Active      | site currently operating          |
| Servicing   | site temporarily offline for work |
| Suspended   | temporarily inactive              |
| Retired     | permanently closed                |
*/