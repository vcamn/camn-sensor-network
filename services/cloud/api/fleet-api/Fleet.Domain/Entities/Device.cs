using System;

namespace Fleet.Domain.Entities
{
    public class Device
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid StationId { get; set; }

        public Guid DeviceStatusId { get; set; }

        public string DeviceIdentifier { get; set; }

        public string DeviceType { get; set; }

        public string SerialNumber { get; set; }

        public string MacAddress { get; set; }

        public string IoTHubDeviceId { get; set; }

        public string FirmwareVersion { get; set; }

        public DateTime? LastActiveAtUtc { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public DateTime UpdatedAtUtc { get; set; }

        public DeviceStatus DeviceStatus { get; set; }

        public Station Station { get; set; }

        public ICollection<Sensor> Sensors { get; set; } = [];
    }
}