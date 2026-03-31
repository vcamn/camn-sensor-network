using System;

namespace Fleet.Domain.Constants;

public static class SensorStatusIds
{
    public static readonly Guid Active = Guid.Parse("8cc727d7-14f1-4169-a5e2-1cf453347628");

    public static readonly Guid Inactive = Guid.Parse("30a5c8d5-2681-4668-8152-6f6a7fbd8851");

    public static readonly Guid Offline = Guid.Parse("e1aeed49-8393-43e5-be60-4c9ef9f6726a");

    public static readonly Guid Error = Guid.Parse("78f451e3-9ea6-49b9-8dee-5c62dce5dab1");

    public static readonly Guid Provisioning = Guid.Parse("804eb64f-db79-4945-8182-105f208ec543");
    
    public static readonly Guid Maintenance = Guid.Parse("5d5c961e-2ed8-4f81-b576-4aae7798b070");
    
    public static readonly Guid Retired = Guid.Parse("a3b059d6-6b54-446f-97ab-78600a5a2a9e");
}
