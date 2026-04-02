using System;

namespace Fleet.Infrastructure.Persistence.SeedData;

public static class SiteStatusSeed
{
    public static readonly (Guid Id, string Code, string Name, string Description, int DisplayOrder)[] Data = new[]
    {
        (Guid.NewGuid(), "PROPOSED", "Proposed", "Site is proposed", 1),
        (Guid.NewGuid(), "APPROVED", "Approved", "Site approved for deployment", 2),
        (Guid.NewGuid(), "ACTIVE", "Active", "Site operational", 3),
        (Guid.NewGuid(), "SERVICING", "Servicing", "Site under maintenance", 4),
        (Guid.NewGuid(), "RETIRED", "Retired", "Site retired", 5)
    };
}
