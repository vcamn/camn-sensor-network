using System;

namespace Fleet.Domain.Constants;

public static class StationStatusIds
{
    public static readonly Guid Active = Guid.Parse("e6028031-2bf3-4a6c-a822-027f2a946c3c");
    public static readonly Guid Inactive = Guid.Parse("b3a7971f-8156-4947-bd5b-780b3464efb5");
    public static readonly Guid Offline = Guid.Parse("f1c0a08c-73fd-4c4c-bed5-b3db7ee5c636");
    public static readonly Guid Maintenance = Guid.Parse("c1850ac9-67c7-4459-ac6f-69733da2f946");
    public static readonly Guid Decommissioned = Guid.Parse("4a47a7ac-5917-4720-857f-1e688cf2cb8d");
}
