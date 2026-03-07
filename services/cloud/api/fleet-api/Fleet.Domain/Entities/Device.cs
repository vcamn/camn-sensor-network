using System;

namespace Fleet.Domain.Entities
{
    public class Device
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid StationId { get; set; }

        public string DeviceIdentifier { get; set; }

        public string DeviceType { get; set; }

        public string SerialNumber { get; set; }

        public string MacAddress { get; set; }

        public string IoTHubDeviceId { get; set; }

        public string FirmwareVersion { get; set; }

        public DateTime? LastHeartbeatUtc { get; set; }

        public Station Station { get; set; }

        public ICollection<Sensor> Sensors { get; set; }
    }
}