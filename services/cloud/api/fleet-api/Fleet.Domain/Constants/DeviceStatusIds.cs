using System;

namespace Fleet.Domain.Constants;

public static class DeviceStatusIds
{
    public static readonly Guid Active = Guid.Parse("d6bd5ad1-1530-4aa7-9f05-582ddaa00ca0");

    public static readonly Guid Inactive = Guid.Parse("8850cbf4-45fa-45d5-8d9d-c1c8981889e4");

    public static readonly Guid Offline = Guid.Parse("7a00a588-f865-4ba2-87e3-48744c1c3df1");

    public static readonly Guid Error = Guid.Parse("b06ee18a-fafa-4506-8a6e-79bcd50f9dc9");

    public static readonly Guid Provisioning = Guid.Parse("1c4824e9-5570-472b-966f-73f8ff9e3adb");
    
    public static readonly Guid Maintenance = Guid.Parse("a72661c3-0250-4cf0-958e-286880d2bdcc");
    
    public static readonly Guid Retired = Guid.Parse("d0ab3438-6b44-48de-a61a-4073d78254f4");
}
